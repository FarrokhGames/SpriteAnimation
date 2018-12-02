using System.Collections.Generic;
using System.Linq;
using FarrokhGames.SpriteAnimation.Frame;
using NSubstitute;
using NUnit.Framework;

namespace FarrokhGames.SpriteAnimation
{
    [TestFixture, Category("Animation")]
    public class FrameAnimatorTests
    {
        IFrameAnimator _animator;
        IClip _clip;

        int _lastIndex = -1;
        int _onFrameChangedCount = 0;
        int _onCompleteCount = 0;
        string _lastTrigger;
        int _onTriggerCount = 0;

        [SetUp]
        public void SetupTests()
        {
            _clip = Substitute.For<IClip>();
            _clip.FrameRate.Returns(1);
            _clip.Name.Returns("clip");
            _clip.Length.Returns(3);
            var frame0 = CreateFrame(1, 1f);
            _clip[0].Returns(frame0);
            var frame1 = CreateFrame(2, 8f, "trigger");
            _clip[1].Returns(frame1);
            var frame2 = CreateFrame(3, 1f);
            _clip[2].Returns(frame2);

            _animator = new FrameAnimator();

            _lastIndex = -1;
            _onFrameChangedCount = 0;
            _animator.OnFrameChanged += (frame) =>
            {
                _lastIndex = frame;
                _onFrameChangedCount++;
            };

            _onCompleteCount = 0;
            _animator.OnClipComplete += () => _onCompleteCount++;

            _lastTrigger = string.Empty;
            _animator.OnTrigger += triggerName =>
            {
                _lastTrigger = triggerName;
                _onTriggerCount++;
            };
        }

        IFrame CreateFrame(int index, float speed = 1f, string trigger = "")
        {
            var frame = Substitute.For<IFrame>();
            frame.Index.Returns(index);
            frame.Speed.Returns(speed);
            frame.TriggerName.Returns(trigger);
            return frame;
        }

        void Tick(int itterations = 100, float deltaTime = 1f)
        {
            for (var i = 0; i < itterations; i++)
            {
                _animator.Tick(deltaTime);
            }
        }

        ///////////////////////
        ///  Initial State  ///
        ///////////////////////

        [Test]
        public void Init_CurrentClip_IsNull()
        {
            Assert.That(_animator.CurrentClip, Is.Null);
        }

        [Test]
        public void Init_IsPlaying_IsFalse()
        {
            Assert.That(_animator.IsPlaying, Is.False);
        }

        //////////////////// 
        ///     Play     ///
        ////////////////////

        [Test]
        public void Play_NullClip_NothingHappens()
        {
            _animator.Play(null);
            Assert.That(_animator.IsPlaying, Is.False);
            Assert.That(_animator.CurrentClip, Is.Null);
        }

        [Test]
        public void Play_SetsCurrentClip()
        {
            _animator.Play(_clip);
            Assert.That(_animator.CurrentClip, Is.SameAs(_clip));
        }

        [Test]
        public void Play_StartsPlaying()
        {
            _animator.Play(_clip);
            Assert.That(_animator.IsPlaying, Is.True);
        }

        [Test]
        public void Play_OnFrameChanged()
        {
            _animator.Play(_clip);
            Assert.That(_onFrameChangedCount, Is.EqualTo(1));
            Assert.That(_lastIndex, Is.EqualTo(0));
        }

        [Test]
        public void Play_OnFrameChanged_RandomStart()
        {
            var _startingFrames = new List<int>();
            _clip.RandomStart.Returns(true);
            for (var i = 0; i < 100; i++)
            {
                var animator = new FrameAnimator();
                animator.OnFrameChanged += (index) =>
                {
                    _startingFrames.Add(index);
                };
                animator.Play(_clip);
            }
            var distinctCunt = _startingFrames.Distinct().Count();
            Assert.That(distinctCunt, Is.GreaterThan(1));
        }

        [Test]
        public void Play_SameClipTwice_NothingHappens()
        {
            _animator.Play(_clip);
            Tick(2);
            _onFrameChangedCount = 0;
            _animator.Play(_clip);
            Assert.That(_onFrameChangedCount, Is.Zero);
            Assert.That(_lastIndex, Is.EqualTo(2));
        }

        [Test]
        public void Play_Looping()
        {
            _clip.Loop.Returns(true);
            _animator.Play(_clip);
            Tick(30);
            Assert.That(_onFrameChangedCount, Is.EqualTo(31));
        }

        [Test]
        public void Play_NonLooping()
        {
            _animator.Play(_clip);
            Tick();
            Assert.That(_onFrameChangedCount, Is.EqualTo(3));
        }

        /////////////////// 
        ///    Pause    ///
        ///////////////////

        [Test]
        public void Pause_NullClip_NothingHappens()
        {
            _animator.Pause();
        }

        [Test]
        public void Pause_StopsPlaying()
        {
            _animator.Play(_clip);
            Assert.That(_animator.IsPlaying, Is.True);
            _animator.Pause();
            Assert.That(_animator.IsPlaying, Is.False);
        }

        //////////////////// 
        ///    Resume    ///
        ////////////////////

        [Test]
        public void Resume_NullClip_NothingHappens()
        {
            _animator.Resume();
        }

        [Test]
        public void Resume__StartsPlaying()
        {
            _animator.Play(_clip);
            Assert.That(_animator.IsPlaying, Is.True);
            _animator.Pause();
            Assert.That(_animator.IsPlaying, Is.False);
            _animator.Resume();
            Assert.That(_animator.IsPlaying, Is.True);
        }

        //////////////////// 
        ///  OnComplete  ///
        ////////////////////

        [Test]
        public void OnComplete_NotCalledForLoopingClip()
        {
            _clip.Loop.Returns(true);
            _animator.Play(_clip);
            Tick();
            Assert.That(_onCompleteCount, Is.Zero);
        }

        [Test]
        public void OnComplete()
        {
            _animator.Play(_clip);
            Tick();
            Assert.That(_onCompleteCount, Is.EqualTo(1));
        }

        /////////////////// 
        ///  OnTrigger  ///
        ///////////////////
        /// 
        [Test]
        public void OnTrigger()
        {
            _animator.Play(_clip);
            Tick();
            Assert.That(_onCompleteCount, Is.EqualTo(1));
        }

        /* 
        Action OnClipComplete { get; set; }
        Action<int> OnFrameChanged { get; set; }
        Action<string> OnTrigger { get; set; }
        void Play(IClip clip);
        void Pause();
        void Resume();
        bool IsPlaying { get; }
        IClip CurrentClip { get; }
        void Tick(float deltaTime);
        */
    }
}