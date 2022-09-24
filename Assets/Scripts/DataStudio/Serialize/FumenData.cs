using System;
using System.Collections;
using System.Collections.Generic;

namespace Runner.DataStudio.Serialize
{
    public enum LayerType { Start, Normal, End, Null = -1 }
    public enum CollectionType { Brick, Diamond, Null = -1 }
    public enum ObstacleType { Jump, Slide, Kick, Lantern, Missile, Reverse, Null = -1 }
    public enum PositionType { Normal, Up, Sky, Null = -1 }
    public enum CompositionType { Fly, Boss, Tutorial, Credit}

    public record BaseObject : IComparable<BaseObject>
    {
        public uint id;
        public uint met, submet;
        public float time;

        public int CompareTo(BaseObject other)
        {
            if (other.met == met && other.submet == submet) return 0;
            else if (other.met == met && other.submet < submet) return 1;
            else if (other.met < met) return 1;
            return -1;
        }
    }
    public record BaseFieldObject : BaseObject
    {
        public PositionType positionType;
    }

    public record Composition : BaseObject
    {
        public CompositionType compositionType;
        public uint para;
        public bool isFinished = false;
    }

    public record FlyComposition : Composition
    {
        public FlyComposition(uint met, uint submet, uint canFly)
        {
            compositionType = CompositionType.Fly;
            this.met = met;
            this.submet = submet;
            para = canFly;
            isFinished = false;
        }
    }

    public record BossComposition : Composition
    {
        public BossComposition(uint met, uint submet, uint showBoss)
        {
            compositionType = CompositionType.Boss;
            this.met = met;
            this.submet = submet;
            para = showBoss;
            isFinished = false;
        }
    }

    public record TutorialComposition : Composition
    {
        public TutorialComposition(uint met, uint submet, uint tutorialID)
        {
            compositionType = CompositionType.Tutorial;
            this.met = met;
            this.submet = submet;
            para = tutorialID;
            isFinished = false;
        }
    }

    public record CreditComposition : Composition
    {
        public CreditComposition(uint met, uint submet, uint creditID)
        {
            compositionType = CompositionType.Credit;
            this.met = met;
            this.submet = submet;
            para = creditID;
            isFinished = false;
        }
    }

    public record Layer : BaseFieldObject
    {
        public LayerType type;
        public float centerPos;
        public float highSpeed;
        public bool haveSpeed = false;
        public float speed;

        public Layer(LayerType type, uint id, uint met, uint submet, float centerPos, float highSpeed, bool haveSpeed, float speed = 0)
        {
            this.type = type;
            this.id = id;
            this.met = met;
            this.submet = submet;
            this.centerPos = centerPos;
            this.highSpeed = highSpeed;
            this.haveSpeed = haveSpeed;
            this.speed = speed;
        }
    }

    public record Collection : BaseFieldObject
    {
        public CollectionType type;
        public uint layerID;
    }

    public record Obstacle : BaseFieldObject
    {
        public ObstacleType type;
        public uint layerID;
    }

    public record Brick : Collection
    {
        public Brick(uint met, uint submet, uint pos)
        {
            type = CollectionType.Brick;
            this.met = met;
            this.submet = submet;
            positionType = (PositionType)pos;
        }
    }

    public record JumpObstacle : Obstacle
    {
        public JumpObstacle(uint met, uint submet, uint pos)
        {
            type = ObstacleType.Jump;
            this.met = met;
            this.submet = submet;
            positionType = (PositionType)pos;
        }

    }

    public record SlideObstacle : Obstacle
    {
        public SlideObstacle(uint met, uint submet, uint pos)
        {
            type = ObstacleType.Slide;
            this.met = met;
            this.submet = submet;
            positionType = (PositionType)pos;
        }

    }
    public record KickObstacle : Obstacle
    {
        public KickObstacle(uint met, uint submet, uint pos)
        {
            type = ObstacleType.Kick;
            this.met = met;
            this.submet = submet;
            positionType = (PositionType)pos;
        }

    }


    public record LanternObstacle : Obstacle
    {
        public LanternObstacle(uint met, uint submet, uint pos)
        {
            type = ObstacleType.Lantern;
            this.met = met;
            this.submet = submet;
            positionType = (PositionType)pos;
        }
    }

    public record MissileObstacle : Obstacle
    {
        public MissileObstacle(uint met, uint submet, uint pos)
        {
            type = ObstacleType.Missile;
            this.met = met;
            this.submet = submet;
            positionType = (PositionType)pos;
        }
    }

    public record ReverseObstacle : Obstacle
    {
        public ReverseObstacle(uint met, uint submet, uint pos)
        {
            type = ObstacleType.Reverse;
            this.met = met;
            this.submet = submet;
            positionType = (PositionType)pos;
        }
    }

    public struct Version : IEquatable<Version>
    {
        public uint major, minor;
        public static Version Version10 { get => new(1, 0); }
        public static Version Version11 { get => new(1, 1); }

        public Version(uint major, uint minor)
        {
            this.major = major;
            this.minor = minor;
        }

        public bool Equals(Version other)
        {
            return major == other.major && minor == other.minor;
        }
    }

    public class FumenData
    {
        public Version version;
        public float mainbpm;
        public int barCount, beatCount;
        public List<Composition> compositions;
        public List<Collection> collections;
        public List<Obstacle> obstacles;
        public List<Layer> layers;
        public float healthMultiply;

        public FumenData()
        {
            compositions = new List<Composition>();
            collections = new List<Collection>();
            obstacles = new List<Obstacle>();
            layers = new List<Layer>();
            healthMultiply = 1.0f;
        }
    }

}
