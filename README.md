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
| AFK | Plays whenever you press END, open the SteamVR Overlay, or take off your headset. (Transforms Only) |
| BACKFLIP | One of the 8 default standing Emotes. (Transforms Only) |
| CROUCH_STILL | Plays whenever you're crouched and not moving. (Transforms Only) |
| CROUCH_WALK_FORWARD | Plays whenever you're crouched and walking forward. (Transforms Only) |
| CROUCH_WALK_RIGHT | Plays whenever you're crouched and walk directly right. \[Mirrored for Left] (Transforms Only) |

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