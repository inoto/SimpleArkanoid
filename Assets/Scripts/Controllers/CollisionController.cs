using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SimpleArkanoid
{
    public class CollisionController : MonoBehaviour
    {
        [SerializeField] Paddle Paddle = null;
        [SerializeField] BoxCollider2D[] Borders;

        [SerializeField] List<Ball> Balls = new List<Ball>();
        Dictionary<Ball, List<BoxCollider2D>> ballsCollisions =
            new Dictionary<Ball, List<BoxCollider2D>>();

        List<Block> blocks = new List<Block>();
        List<Bonus> bonuses = new List<Bonus>();

        Vector2 closest, dist, contact;

        void Start()
        {
            blocks = FindObjectsOfType<Block>().ToList();

            Ball.LaunchedEvent += AddBall;
            Ball.LostEvent += RemoveBall;
            Bonus.SpawnedEvent += AddBonus;
            Bonus.LostEvent += RemoveBonus;
        }

        void OnDestroy()
        {
            Ball.LaunchedEvent -= AddBall;
            Ball.LostEvent -= RemoveBall;
            Bonus.SpawnedEvent -= AddBonus;
            Bonus.LostEvent -= RemoveBonus;
        }

        void AddBall(Ball ball)
        {
            if (!Balls.Contains(ball))
            {
                Balls.Add(ball);
                ballsCollisions.Add(ball, new List<BoxCollider2D>());
            }
        }

        void RemoveBall(Ball ball)
        {
            if (Balls.Contains(ball))
            {
                Balls.Remove(ball);
                ballsCollisions.Remove(ball);
            }
        }

        void AddBonus(Bonus bonus)
        {
            if (!bonuses.Contains(bonus))
            {
                bonuses.Add(bonus);
            }
        }

        void RemoveBonus(Bonus bonus)
        {
            if (bonuses.Contains(bonus))
            {
                bonuses.Remove(bonus);
            }
        }

        void Update()
        {
            ProcessCollisions();
        }

        void ProcessCollisions()
        {
            for (int i = 0; i < Balls.Count; i++)
            {
                // ball - paddle
                if (CircleRectIntersects(Balls[i].Collider, Paddle.Collider))
                {
                    if (!ballsCollisions[Balls[i]].Contains(Paddle.Collider))
                    {
                        ballsCollisions[Balls[i]].Add(Paddle.Collider);
                        Balls[i].BouncePaddle();
                    }
                }
                else
                {
                    if (ballsCollisions[Balls[i]].Contains(Paddle.Collider))
                    {
                        ballsCollisions[Balls[i]].Remove(Paddle.Collider);
                    }
                }
                // ball - borders
                for (int b = 0; b < Borders.Length; b++)
                {
                    // TODO: use out var to get contact point
                    if (CircleRectIntersects(Balls[i].Collider, Borders[b]))
                    {
                        if (!ballsCollisions[Balls[i]].Contains(Borders[b]))
                        {
                            ballsCollisions[Balls[i]].Add(Borders[b]);

                            // TODO: cache bounds and check profiler again
                            contact.x = Mathf.Clamp(Balls[i].transform.position.x,
                                Borders[b].bounds.min.x, Borders[b].bounds.max.x);
                            contact.y = Mathf.Clamp(Balls[i].transform.position.y,
                                Borders[b].bounds.min.y, Borders[b].bounds.max.y);

                            Balls[i].Bounce(contact, GetNormal(Borders[b].bounds,
                                contact, Balls[i].transform.position).normalized);
                        }
                    }
                    else
                    {
                        if (ballsCollisions[Balls[i]].Contains(Borders[b]))
                        {
                            ballsCollisions[Balls[i]].Remove(Borders[b]);
                        }
                    }
                }
                // ball - blocks
                for (int bl = 0; bl < blocks.Count; bl++)
                {
                    if (CircleRectIntersects(Balls[i].Collider, blocks[bl].Collider))
                    {
                        if (!ballsCollisions[Balls[i]].Contains(blocks[bl].Collider))
                        {
                            ballsCollisions[Balls[i]].Add(blocks[bl].Collider);

                            contact.x = Mathf.Clamp(Balls[i].transform.position.x,
                                blocks[bl].Collider.bounds.min.x, blocks[bl].Collider.bounds.max.x);
                            contact.y = Mathf.Clamp(Balls[i].transform.position.y,
                                blocks[bl].Collider.bounds.min.y, blocks[bl].Collider.bounds.max.y);

                            Balls[i].Bounce(contact, GetNormal(blocks[bl].Collider.bounds,
                                contact, Balls[i].transform.position).normalized);
                            if (blocks[bl].Touch())
                            {
                                ballsCollisions[Balls[i]].Remove(blocks[bl].Collider);
                                blocks.Remove(blocks[bl]);
                            }
                        }
                    }
                    else
                    {
                        if (ballsCollisions[Balls[i]].Contains(blocks[bl].Collider))
                        {
                            ballsCollisions[Balls[i]].Remove(blocks[bl].Collider);
                        }
                    }
                }
            }
            for (int i = 0; i < bonuses.Count; i++)
            {
                // bonus - paddle
                if (bonuses[i].Collider.bounds.Intersects(Paddle.Collider.bounds))
                {
                    bonuses[i].Touch();
                    bonuses.RemoveAt(i);
                }
            }
        }

        bool CircleRectIntersects(CircleCollider2D circle, BoxCollider2D rect)
        {
            // Find the closest point to the circle within the rectangle
            float closestX = Mathf.Clamp(circle.transform.position.x,
                rect.bounds.min.x, rect.bounds.max.x);
            float closestY = Mathf.Clamp(circle.transform.position.y,
                rect.bounds.min.y, rect.bounds.max.y);

            // Calculate the distance between the circle's center and this closest point
            float distanceX = circle.transform.position.x - closestX;
            float distanceY = circle.transform.position.y - closestY;

            // If the distance is less than the circle's radius, an intersection occurs
            float distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);
            return distanceSquared < (circle.radius * 2) * (circle.radius * 2);
        }

        Vector2 GetNormal(Bounds b, Vector2 c, Vector2 p)
        {
            closest = ClosestPoint(b, c);
            dist = closest - c;

            float signX = b.center.x > closest.x ? -1 : 1;
            float signY = b.center.y > closest.y ? -1 : 1;

            return new Vector2(Mathf.Abs(dist.y) * signX, Mathf.Abs(dist.x) * signY);
        }

        Vector2 ClosestPoint(Bounds b, Vector2 p)
        {
            Vector2[] points = new Vector2[4];
            float minDist = Single.MaxValue;
            Vector2 result = Vector2.zero;

            Vector2 min = b.min;
            Vector2 max = b.max;

            points[0] = new Vector2(min.x, min.y);
            points[1] = new Vector2(min.x, max.y);
            points[2] = new Vector2(max.x, min.y);
            points[3] = new Vector2(max.x, max.y);

            for (int i = 0; i < points.Length; i++)
            {
                float dist = Vector2.Distance(points[i], p);
                if (dist < minDist)
                {
                    minDist = dist;
                    result = points[i];
                }
            }

            return result;
        }
    }
}