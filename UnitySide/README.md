# Welcome to ACT Documentation

The Avatar Controller Toolkit (ACT) is a tool developed to enhance the visualization of any expression model which would be performed by an agent, Lou, we have created using Blender. 
The ACT project uses the power of Unity and Blender, mixed in a tool made for the users. 
The advantage of our project is you have no constraints for the choice of model you want to use. You can plug your own model on the server and use the avatar.

## About The Project

---
The aim of the project is to be able to plug any model to our server to have a visualization of your data on the avatar.

We have three possible uses of the project :
- You can use Live Stream to view live the avatar move with the data of your model. You can also use Lip Sync and configure your own phoneme profil. You can also record your expression directly on live and use it after.
- You can Record video with your own expression record on Live Stream or import them. You have a timeline to place expression where you want to play them and record them to have a video of your different expressions.
- You can use Wizard of Oz. If you are not familiar with wizard of oz experiments, please follow this [link](https://en.wikipedia.org/wiki/Wizard_of_Oz_experiment). You create a server and you connect to it. You can choose what expression you want to play. The expression can be import or can record on live.

The Record and the WoZ are in progress. In Live Stream, Live, record and lip sync are usable. You can choose audio input device and also enable and disable lip sync but you can't add, modify or delete phomene.

In the futur, we want to improve the toolkit's Record and WoZ, with a better stability for the avatar. We also want to be able to add your own avatar, have a API systeme to make using your model with the avatar more functional and practical and respond to the need of the scientific community.

## How To Use :

---
First, you have to start the ACT Server :

![](https://github.com/Arthur-P0/ACT/blob/ACT_experimental/Unity%20Side/README_Img/GIF_Start_Serveur.gif)

After that, you lauch the unity build :

![](https://github.com/Arthur-P0/ACT/blob/ACT_experimental/Unity%20Side/README_Img/Gif_Unity_Start.gif)

Here you have 4 choice :

- Configuration :
  You can configurate your avatar here :

![](https://github.com/Arthur-P0/ACT/blob/ACT_experimental/Unity%20Side/README_Img/GIF_Avatar_Configuration.gif)

  In the first part, you can change neck and head correction, transition duration and if you want the total expression or not.
  In the second part, you can add, edit and delete your own action. You have to import a CSV with all your action unit.


- Record : Here, you can record your own action previously update on Configuration.

![](https://github.com/Arthur-P0/ACT/blob/ACT_experimental/Unity%20Side/README_Img/GIF_Avatar_Record.gif)

  You have a control pannel, where all action are register. You can drag and drop an action in the queue to create your own record.
  You can adjust head and neck rotation to have the best possible record.


- Wizard Of Oz : Here, you can create a room and connect to it to use expression record before.

![](https://github.com/Arthur-P0/ACT/blob/ACT_experimental/Unity%20Side/README_Img/GIF_Avatar_WoZ.gif)

  All the expression record are on your left, you can press on it to see the expression in action.


- LiveStreaming : 
> [!WARNING] 
> Before launch live Streaming, you have to start Openface so that server can have the data send by openface to LiveStream your expression.

![](https://github.com/Arthur-P0/ACT/blob/ACT_experimental/Unity%20Side/README_Img/Lips-Sync%20Demo.gif)

You can record your expression in realtime, and you can change the parameter of lips sync like phoneme, your mic or enable diseable your mic.

## How does it work 

---

We use openface, who sends these action unit to our server. In the server, the action unit are transform to BlendShape expression. One action unit is linked to several BlendShape.
We take the value of the action unit and we adjust it to work on Unity BlenShape. After this conversion, we send a Json File with all BlendShape name and value and we apply it to the avatar.

## How to modify the avatar

---

For integrate your own avatar, you have to follow these steps :

- When you create your avatar, you need to add BlendShape with the name below so that action unit are redirected to the right BlendShape :
```txt
Expressions_abdomExpansion_max
Expressions_abdomExpansion_min
Expressions_browOutVertL_max
Expressions_browOutVertL_min
Expressions_browOutVertR_max
Expressions_browOutVertR_min
Expressions_browSqueezeL_max
Expressions_browSqueezeL_min
Expressions_browSqueezeR_max
Expressions_browSqueezeR_min
Expressions_browsMidVert_max
Expressions_browsMidVert_min
Expressions_cheekSneerL_max
Expressions_cheekSneerR_max
Expressions_chestExpansion_max
Expressions_chestExpansion_min
Expressions_eyeClosedL_max
Expressions_deglutition_max
Expressions_deglutition_min
Expressions_eyeClosedL_max
Expressions_eyeClosedL_min
Expressions_eyeClosedPressureL_max
Expressions_eyeClosedPressureL_min
Expressions_eyeClosedPressureR_max
Expressions_eyeClosedPressureR_min
Expressions_eyeClosedR_max
Expressions_eyeClosedR_min
Expressions_eyeSquintL_max
Expressions_eyeSquintL_min
Expressions_eyeSquintR_max
Expressions_eyeSquintR_min
Expressions_eyesHoriz_max
Expressions_eyesHoriz_min
Expressions_eyesVert_max
Expressions_eyesVert_min
Expressions_jawHoriz_max
Expressions_jawHoriz_min
Expressions_jawOut_max
Expressions_jawOut_min
Expressions_mouthBite_max
Expressions_mouthBite_min
Expressions_mouthChew_max
Expressions_mouthChew_min
Expressions_mouthClosed_max
Expressions_mouthClosed_min
Expressions_mouthHoriz_max
Expressions_mouthHoriz_min
Expressions_mouthInflated_max
Expressions_mouthInflated_min
Expressions_mouthLowerOut_max
Expressions_mouthLowerOut_min
Expressions_mouthOpenAggr_max
Expressions_mouthOpenAggr_min
Expressions_mouthOpenHalf_max
Expressions_mouthOpenLarge_max
Expressions_mouthOpenLarge_min
Expressions_mouthOpenO_max
Expressions_mouthOpenO_min
Expressions_mouthOpenTeethClosed_max
Expressions_mouthOpenTeethClosed_min
Expressions_mouthOpen_max
Expressions_mouthOpen_min
Expressions_mouthSmileL_max
Expressions_mouthSmileOpen2_max
Expressions_mouthSmileOpen2_min
Expressions_mouthSmileOpen_max
Expressions_mouthSmileOpen_min
Expressions_mouthSmileR_max
Expressions_mouthSmile_max
Expressions_mouthSmile_min
Expressions_nostrilsExpansion_max
Expressions_nostrilsExpansion_min
Expressions_pupilsDilatation_max
Expressions_pupilsDilatation_min
Expressions_tongueHoriz_max
Expressions_tongueHoriz_min
Expressions_tongueOutPressure_max
Expressions_tongueOut_max
Expressions_tongueOut_min
Expressions_tongueTipUp_max
Expressions_tongueVert_max
Expressions_tongueVert_min
```

- Import your avatar in Unity and add the head, the head joint, the neck joint, the left and the right eye joint in the component _Script. Like you can see bellow :
![](https://github.com/Arthur-P0/ACT/blob/ACT_experimental/Unity%20Side/README_Img/Add_Avatar.png)

- If you want to use lips sync, you have to add the lips sync component in the avatar. In your avatar prefab, add the component ULipSync BlendShape. In the phoneme part of the component, create a phoneme for each phoneme BlendShape you have in your avatar. You can see an example bellow :
  ![](https://github.com/Arthur-P0/ACT/blob/ACT_experimental/Unity%20Side/README_Img/phoneme_avatar.png)

In the avatar prefab, create empty gameobject and add a audio source component with no audioclip. Add ULipsync component and Ulipsync Microphone to use your mic for the lipsync. If you want to know more about lipsync, you can follow this [link](https://github.com/hecomi/uLipSync?tab=readme-ov-file)

## TODO
___

1. A functionnal menu to change, add, modify and delete the phoneme of the lipsync
2. A better Avatar with more BlendShape
3. A better stability for the avatar

##  How to contribute
___
1. Fork the project
2. Create a new branch (`git checkout -b feature/featureName`)
3. Commit your changes (`git commit -m 'Add some feature'`)
4. Push to the branch (`git push origin feature/featureName`)
5. Create a new Pull Request
6. Wait for the review and approval

## Contact
___
- Kevin El Haddad - ACT Creator
- [Pierre-Luc MILLET](https://github.com/Pierre-LucM)- pierre-luc.millet@student.junia.com - ACT Dev
- [Arthur PINEAU](https://github.com/Arthur-P0) - arthur.pineau@student.junia.com - ACT Dev
- [Armand DEFFRENNES](https://github.com/JambonPasFrais) - armand.deffrennes@student.junia.com - ACT Dev
=======
## Openface 

You will need openface with a websocket support to use ACT. You will find it at [Openface Websocket](https://github.com/numediart/openface_act)