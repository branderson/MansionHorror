using Assets.AI;
using UnityEngine;

namespace Assets.Game.Enemies
{
    public class ChairController : EnemyController
    {
        public bool MoveTowardsCharacter(GameObject Player, float AttackRange, float Speed, ChairAI.Attack Type)
        {
            if (IsCharacterWithinAttackRange(Player, AttackRange))
            {
                return true;
            }
            Vector3 Direction = Player.transform.position - transform.position;
            Direction.z = 0;
            Direction.Normalize();
            Direction *= AttackRange;
            return MoveTowardsPosition(Player.transform.position - Direction, Speed, Type);
        }

        public bool MoveTowardsPosition(Vector2 Position, float Speed, ChairAI.Attack Type)
        {
            int angle = 0;
            switch (Type)
            {
                case ChairAI.Attack.Left:
                    angle = 20;
                    break;
                case ChairAI.Attack.Right:
                    angle = -20;
                    break;
                case ChairAI.Attack.Wait:
                    return false;
            }
            Vector3 NewPosition = new Vector3(Position.x, Position.y, 0f);
            if (transform.position == NewPosition)
            {
                return true;
            }
            bool left, down, newLeft, newDown;
            NewPosition -= transform.position;
            Vector3 Direction = NewPosition.normalized;
            left = Direction.x < 0 ? true : false;
            down = Direction.y < 0 ? true : false;
            Direction = Quaternion.Euler(0, 0, angle) * Direction;
            Direction *= Speed * Time.deltaTime;
            newLeft = Position.x - (transform.position.x + Direction.x) < 0 ? true : false;
            newDown = Position.y - (transform.position.y + Direction.y) < 0 ? true : false;
            //If we overshoot, go directly to target
            if (((left ^ newLeft) || Direction.x == 0) && ((down ^ newDown) || Direction.y == 0) || (transform.position.x + Direction.x == Position.x && transform.position.y + Direction.y == Position.y))
            {
                Direction.x = Position.x - transform.position.x;
                Direction.y = Position.y - transform.position.y;
                transform.Translate(Direction);
                return true;
            }
            transform.Translate(Direction);
            return false;
        }
    }
}