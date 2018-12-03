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
            _clip.FrameRate.Returns(1f);
            _clip.Name.Returns("clip");
            _clip.FrameCount.Returns(3);
            var frame0 = CreateFrame(1, 1f);
            _clip[0].Returns(frame0);
            var frame1 = CreateFrame(2, 0.5f, "trigger");
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

            _onTriggerCount = 0;
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
            IClip clip = null;
            _animator.Play(clip);
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
            Tick(3);
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
            Tick();
            Assert.That(_onFrameChangedCount, Is.EqualTo(76));
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
            Assert.DoesNotThrow(() => _animator.Pause());
        }

        [Test]
        public void Pause_StopsPlaying()
        {
            _animator.Play(_clip);
            Assert.That(_animator.IsPlaying, Is.True);
            _animator.Pause();
            Assert.That(_animator.IsPlaying, Is.False);
        }

        [Test]
        public void Pause_CurrentClipStaysTheSame()
        {
            _animator.Play(_clip);
            _animator.Pause();
            Assert.That(_animator.CurrentClip, Is.SameAs(_clip));
        }

        //////////////////// 
        ///    Resume    ///
        ////////////////////

        [Test]
        public void Resume_NullClip_NothingHappens()
        {
            Assert.DoesNotThrow(() => _animator.Resume());
        }

        [Test]
        public void Resume_StartsPlaying()
        {
            _animator.Play(_clip);
            Assert.That(_animator.IsPlaying, Is.True);
            _animator.Pause();
            Assert.That(_animator.IsPlaying, Is.False);
            _animator.Resume();
            Assert.That(_animator.IsPlaying, Is.True);
        }

        [Test]
        public void Resume_CurrentClipStaysTheSame()
        {
            _animator.Play(_clip);
            _animator.Pause();
            _animator.Resume();
            Assert.That(_animator.CurrentClip, Is.SameAs(_clip));
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

        [Test]
        public void OnTrigger_CalledOnceForNonLooping()
        {
            _clip.Loop.Returns(false);
            _animator.Play(_clip);
            Tick();
            Assert.That(_onTriggerCount, Is.EqualTo(1));
            Assert.That(_lastTrigger, Is.EqualTo("trigger"));
        }

        [Test]
        public void OnTrigger_CalledEachLoop()
        {
            _clip.Loop.Returns(true);
            _animator.Play(_clip);
            Tick(4 * 10, 1f);
            Assert.That(_onTriggerCount, Is.EqualTo(10));
            Assert.That(_lastTrigger, Is.EqualTo("trigger"));
        }
    }
}