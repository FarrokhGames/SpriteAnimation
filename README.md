## A Unity3D-based sprite animator system for simple and advanced animation setups

<img src="Documentation/animation.gif?raw=true" alt="Animation Example" width="512px" height="512px"/>

## Table of Contents

- <a href="#features">Features</a>
- <a href="#installation">Installation</a>
- <a href="#examples">Example</a>
- <a href="#documentation">Documentation</a>
  - <a href="#gettingstarted">Getting Started</a>
  - <a href="#theanimators">The Animators<a>
  - <a href="#clipsandframes">Settings, Clips & Frames<a>
  - <a href="#otherfiles">Other files included<a>
- <a href="#license">License</a>

---

## <a id="features"></a>Features

- ```Create sprite based animations```
- ```Share animation clips``` between different animators
- Easily ```flip``` your sprites or group of sprites
- Add ```triggers``` that can be listened to, for things like playing footstep sounds
- Control the ```frame rate``` of each clip and its individual frames
- Create ```advanced setups``` of co-dependent animators, particle effects and containers. Where parent animators can control their children.

---

## <a id="installation"></a>Installation

Simply copy the folder "```Assets/Plugins```" into your project and you're good to go. Optionally, you can add the folder "```Assets/Examples```" to get started right away.
Unit Tests are located in the "```Assets/Editor/Tests```"-folder.

---

## <a id="examples"></a>Examples

A fully functional example is included with this reposetory and can be found in the folder "```Assets/Examples```". 
Use the ```left and right arrow keys``` to toggle between an idle and walk animation.

- ```ImageAnimator.scene``` - the Unity Scene that contains an example using the Image Animator (UI).
- ```SpriteAnimator.scene``` - the Unity Scene that contains an example using the Sprite Animator (2D).
- ```AnimationController.cs``` - a ```MonoBehaviour``` that plays animations.
- ```spritesheet.png``` - Contains the artwork and animations used.
- ```Audio-folder``` - Contains footstep audio used in the example.

---

## <a id="documentation"></a>Documentation

Below you can find documentation of various parts of the system. You are encouraged to look through the code, where more in-depth code docs can be found.

---

### <a id="gettingstarted"></a>Getting Started

**For 2D SpriteRenderers**, simply create a new gameobject and add ```SpriteAnimator.cs``` as a component.<br>
**For UI-Canvas Images**, simply create a new gameobject in your canvas and add ```ImageAnimator.cs``` as a component.

---

### <a id='theanimators'></a>The Animators

There are several animator behaviors included with this plugin.

| Script                        | Description                                                   |
| ----------------------------- |---------------------------------------------------------------|
|```SpriteAnimator.cs```        | For animating sprites in 2D using a SpriteRenderer |
|```ImageAnimator.cs```         | For animating sprites using in the UI-Canvas using an Image |
|```ParticleSystemAnimator.cs```| For connecting a Particle System to other animators, making it play and stop depending on what animation is being played by its parent |
|```TransformAnimator.cs```     | For connecting a Transform to other animators, making it show and hide depending on what animation is being played by its parent |
<br>
<img src="Documentation/hierarchy.png?raw=true" alt="Hierarchy" width="361px"/><br>
Here is the hiearchy-setup of the example, where all animators are in use.

---

Below is a list of actions methods and getters within ```IAnimator```-interface.
```cs
/// <summary>
/// Invoked when the current clip has completed its animation
/// </summary>
Action OnClipComplete { get; set; }

/// <summary>
/// Invoked when playing a frame with a trigger on it
/// </summary>
Action<string> OnTrigger { get; set; }

/// <summary>
/// Returns true if an animation is currently playing
/// </summary>
bool IsPlaying { get; }

/// <summary>
/// Returns the current clip
/// </summary>
IClip CurrentClip { get; }

/// <summary>
/// Plays the given clip
/// </summary>
/// <param name="clip">The clip to play</param>
void Play(IClip clip);

/// <summary>
/// Tries to find and play a clip of given name
/// </summary>
/// <param name="clipName">The name of the clip to play</param>
bool Play(string clipName);

/// <summary>
/// Pauses the current animation
/// </summary>
void Pause();

/// <summary>
/// Resumes the current animation
/// </summary>
void Resume();

/// <summary>
/// Gets or sets wether to flip this animator or not
/// </summary>
bool Flip { get; set; }

/// <summary>
/// How to handle the relationship with a parent animator
/// </summary>
AnimatorChildMode ChildMode { get; }
```
---

### <a id="clipsandframes"></a>Settings, Clips & Frames

Below is a run down of the general elements, and their settings, of the animator-behaviors.

#### Sprites

<img src="Documentation/inspector-sprites.png?raw=true" alt="Sprites" width="361px"/>

The sprites-list stores all sprites accosiated with this animator, and decouple the frame-reference from the actual animations. Adding multple sprites at once can be done by first locking the inspector, and then dragging multiple sprites inot the sprites-list.

#### Clips

<img src="Documentation/inspector-clip.png?raw=true" alt="Clips" width="361px"/>

- ```Name:``` The name of the clip.
- ```Loop:``` Wether this clip should loop or not *(please note that looping clipts do not invoke OnClipComplete)*.
- ```Random Start:``` Wether this clip should start on a random frame when played.
- ```Frame Rate:``` The frame rate *(in frames per second)* of this animation clip.

#### Frames

<img src="Documentation/inspector-frames.png?raw=true" alt="Frames" width="361px"/>

- ```Index:``` The index of the sprite to use with this frame *(should correspond to the sprite in the sprite-list)*.
- ```Speed:``` The speed-mod of this individual frame *(0 = 0%, 1 = 100%, 2 = 200%, etc)*.
- ```Trigger:``` The trigger-name of this frame. Leave empty if no trigger is desired.

#### Settings

<img src="Documentation/inspector-animatorsettings.png?raw=true" alt="Animator Settings" width="361px"/>

These are the general settings of the animator itself.
- ```Animate Children:``` Wether this animator should pass its animation down to its children. This also affects things like flip and sorting order.
- ```Child Mode:``` Indicates how parents should handle this animator as a child. 
  - ```PlayWithParent:``` Parents will play animations on this child using the animations name *(The child will play its own animation with the same name)*.
  - ```ShareClipsWithParent:``` Parents will share their clips with this child. This is useful for things like character bodyparts and shadow.
  - ```IgnoreParent:``` The parent will not play animations on this child.
- ```Allow Flipping:``` Wether this animator can be flipped or will keep its direction

---

### <a id="otherfiles"></a>Other files included

NSubstitute is used for mocking within the unit tests. More info about NSubstitute can be found <a href="http://nsubstitute.github.io/">here</a>.

---

## <a id="license"></a>License
    MIT License

    Copyright (c) 2018 Farrokh Games

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in all
    copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    SOFTWARE.
