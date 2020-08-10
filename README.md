VRC AV3 Overrides
==============

Author: Joshuarox100

Usage: This package is meant for use with VRChat's Avatars 3.0 (VRCSDK3-AVATAR)

Description: Make upgrading to 3.0 a breeze with AV3 Overrides!

Setting Up
--------------

1) Download and import the latest **Unity Package** from [**Releases**](https://github.com/Joshuarox100/VRC-AV3-Overrides/releases) on GitHub **(You will have issues if you don't)**.

<p align="center">
  <img width="80%" height="80%" src="">
</p>

2) Duplicate the AV3 Override Controller located in the AV3 Overrides folder.

<p align="center">
  <img width="80%" height="80%" src="">
</p>

3) Next, modify the duplicate as you wish with whatever Animations you want.
	>A list of what each Animation is for (and what types are compatible) can be found [here](#av3-override-controller).

<p align="center">
  <img width="80%" height="80%" src="">
</p>

4) Once you have it how you like, open the [Conversion Window](#conversion-window) found under Window -> AV3 Tools -> AV3 Overrides. 

<p align="center">
  <img width="80%" height="80%" src="">
</p>

5. Configure the provided settings as you'd like, drag in your duplicate Controller, and click Generate to convert it into Animators you can use in the Avatar Descriptor.
	>Upon a successful generation, the created Animators will automatically be slotted into the descriptor in their corresponding spots (unless if one is already present and you haven't turned on Replace Animators).

Everything should now be fully set up! If you have any issues or questions, look in the [troubleshooting](#troubleshooting) and [questions](#common-questions) section below before [contacting me](#contacting-me).

Conversion Window
--------------
<p align="center">
  <img width="60%" height="60%" src="">
</p>

| Setting | Function |
| --------- | ---------- |
| Active Avatar | The Avatar you want to convert an override for. |
| Custom Override | The AV3 Override Controller to be converted. |
| Replace Animators | Replace Animators already present in the Avatar Descriptor. |
| Destination | The folder where generated files will be saved to. |
| Overwrite All | Automatically overwrite existing files if needed. |

AV3 Override Controller
--------------
<p align="center">
  <img width="60%" height="60%" src="">
</p>

| Animation | Purpose |
| :-------: | ---------- |
| AFK | Plays whenever you press END, open the SteamVR Overlay, or take off your headset. **(Transforms Only)** |
| BACKFLIP | One of the 8 default standing Emotes. **(Transforms Only)** |
| CROUCH_STILL | Plays whenever you're crouched and not moving. **(Transforms Only)** |
| CROUCH_WALK_FORWARD | Plays whenever you're crouched and walking forward. **(Transforms Only)** |
| CROUCH_WALK_RIGHT | Plays whenever you're crouched and walk directly right. **\[Mirrored for Left] (Transforms Only)** |
| CROUCH_WALK_RIGHT_135 | Plays whenever you're crouched and walk to the right slightly diagonally. **\[Mirrored for Left] (Transforms Only)** |
| CROUCH_WALK_RIGHT_45 | Plays whenever you're crouched and walk diagonally. **\[Mirrored for Left] (Transforms Only)** |
| DANCE | One of the 8 default standing Emotes. **(Transforms Only)** |
| DIE | One of the 8 default standing Emotes. **(Transforms Only)** |
| EYES_DIE | Plays to close your eyes when doing the DIE Emote. **(Non-Transforms Only)** |
| EYES_OPEN | Plays to open up your eyes when DIE has completed. **(Non-Transforms Only)** |
| EYES_SHUT | Plays to close your eyes when AFK. **(Non-Transforms Only)** |
| FACE_FIST | Plays when gesturing a fist. **(Non-Transforms Only)** |
| FACE_GUN | Plays when gesturing a hand gun. **(Non-Transforms Only)** |
| FACE_IDLE | Plays when you're not gesturing anything. **(Non-Transforms Only)** |
| FACE_OPEN | Plays when gesturing an open hand. **(Non-Transforms Only)** |
| FACE_PEACE | Plays when gesturing a peace sign. **(Non-Transforms Only)** |
| FACE_ROCK | Plays when gesturing rock and roll. **(Non-Transforms Only)** |
| FACE_THUMBS_UP | Plays when gesturing a thumbs up. **(Non-Transforms Only)** |
| FALL_LONG | Plays after falling for a long while. **(Transforms Only)** |
| FALL_SHORT | Plays when you start falling. **(Transforms Only)** |
| HANDS_FIST | Plays when gesturing a fist. **(Transforms Only)** |
| HANDS_GUN | Plays when gesturing a hand gun. **(Transforms Only)** |
| HANDS_IDLE | Plays when you're not gesturing anything. **(Transforms Only)** |
| HANDS_OPEN | Plays when gesturing an open hand. **(Transforms Only)** |
| HANDS_PEACE | Plays when gesturing a peace sign. **(Transforms Only)** |
| HANDS_ROCK | Plays when gesturing rock and roll. **(Transforms Only)** |
| HANDS_THUMBS_UP | Plays when gesturing a thumbs up. **(Transforms Only)** |
| IDLE | Additional movement added to your body while idling. **(Transforms Only)** |
| IKPOSE | Animation used for determining major joint bends. **(Transforms Only)** |
| LANDING | Plays when landing from a jump. **(Transforms Only)** |
| LOW_CRAWL_FORWARD | Plays when prone and moving forward. **(Transforms Only)** |
| LOW_CRAWL_RIGHT | Plays when prone and moving right. **\[Mirrored for Left] (Transforms Only)** |
| LOW_CRAWL_STILL | Plays when prone and not moving. **(Transforms Only)** |
| MOOD_ANGRY | Plays depending on your Avatar's mood. **(Non-Transforms Only)** |
| MOOD_HAPPY | Plays depending on your Avatar's mood. **(Non-Transforms Only)** |
| MOOD_NEUTRAL | Plays depending on your Avatar's mood. **(Non-Transforms Only)** |
| MOOD_SAD | Plays depending on your Avatar's mood. **(Non-Transforms Only)** |
| MOOD_SURPRISED | Plays depending on your Avatar's mood. **(Non-Transforms Only)** |
| RUN_BACKWARD | Plays when running backwards. **(Transforms Only)** |
| RUN_FORWARD | Plays when running forwards. **(Transforms Only)** |
| RUN_STRAFE_RIGHT | Plays when running directly right. **\[Mirrored for Left] (Transforms Only)** |
| RUN_STRAFE_RIGHT_135 | Plays when running right slightly diagonally. **\[Mirrored for Left] (Transforms Only)** |
| RUN_STRAFE_RIGHT_45 | Plays when running diagonally. **\[Mirrored for Left] (Transforms Only)** |
| SEATED_CLAP* | One of the 8 default seated Emotes. **(Transforms Only)** |
| SEATED_DISAPPROVE* | One of the 8 default seated Emotes. **(Transforms Only)** |
| SEATED_DISBELIEF* | One of the 8 default seated Emotes. **(Transforms Only)** |
| SEATED_DRUM* | One of the 8 default seated Emotes. **(Transforms Only)** |
| SEATED_LAUGH* | One of the 8 default seated Emotes. **(Transforms Only)** |
| SEATED_POINT* | One of the 8 default seated Emotes. **(Transforms Only)** |
| SEATED_RAISE_HAND* | One of the 8 default seated Emotes. **(Transforms Only)** |
| SEATED_SHAKE_FIST* | One of the 8 default seated Emotes. **(Transforms Only)** |
| SIT | Plays while sitting. **(Transforms Only)** |
| SPRINT_FORWARD | Plays while sprinting forward. **(Transforms Only)** |
| STAND_CHEER | One of the 8 default standing Emotes. **(Transforms Only)** |
| STAND_CLAP | One of the 8 default standing Emotes. **(Transforms Only)** |
| STAND_POINT | One of the 8 default standing Emotes. **(Transforms Only)** |
| STAND_SADKICK | One of the 8 default standing Emotes. **(Transforms Only)** |
| STAND_STILL | Played while standing still. **(Transforms Only)** |
| STAND_WAVE | One of the 8 default standing Emotes. **(Transforms Only)** |
| STRAFE_RIGHT | Plays when walking directly right. **\[Mirrored for Left] (Transforms Only)** |
| STRAFE_RIGHT_135 | Plays when walking right slightly diagonally. **\[Mirrored for Left] (Transforms Only)** |
| STRAFE_RIGHT_45 | Plays when walking diagonally. **\[Mirrored for Left] (Transforms Only)** |
| SUPINE_GETUP | Played when exiting the DIE Emote. **(Transforms Only)** |
| TPOSE | Used for determining various measurements of your avatar, most notably the position of the viewpoint. **(Transforms Only)** |
| WALK_BACKWARD | Played while walking backwards. **(Transforms Only)** |
| WALK_FORWARD | Played while walking forwards. **(Transforms Only)** |

>\* \- Seated Emotes will not activate without use of a custom Expressions Menu.

Common Questions
--------------
**Can I use this to replace Animations within my own Animators?**
>No, I may release another tool later on for that. This tool is meant simply for upgrading 2.0 Avatars in an easier fashion so you can get on to learning and trying new things that Avatars 3.0 allows for.

Troubleshooting
--------------
**The window refuses to open.**
>To fix this, reset your Editor layout by clicking Layout in the top right of the Editor and clicking Reset Factory Settings, then attempt to open the window again.

**"Your Animation cannot be used for this because it modifies properties unusable in its layer!"**
>This will occur if you're trying to use a Animation that modifies Non-Transforms for an Animation within a layer that doesn't allow it (or vice versa). You can find out which Animations are 'Transforms Only' [here](#av3-override-controller). If your Animation modifies both Transforms and Non-Transforms, you'll need to split the Animation in two.

**"An exception occured!"**
>If this happens, ensure you have a clean install of AV3 Overrides, and if the problem persists, [let me know](#contacting-me)!

Contacting Me
--------------
If you still have some questions or recommendations you'd like to throw my way, you can ask me on Discord (Joshuarox100#5024) or leave a suggestion or issue on the [GitHub](https://github.com/Joshuarox100/VRC-AV3-Overrides/issues) page.