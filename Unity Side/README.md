# Welcome to ACT Documentation
The Avatar Controller Toolkit (ACT) is a tool developed to enhance the visualization of any expression model which would be performed by an agent, Lou, we have created using Blender. 
The ACT project uses the power of Unity and Blender, mixed in a tool made for the users. 
The advantage of our project is you have no constraints for the choice of model you want to use. You can plug your own model on the server and use the avatar.

## About The Project 
The aim of the project is to be able to plug any model to our server to have a visualization of your data on the avatar.

We have three possible uses of the project :
- You can use Live Stream to view live the avatar move with the data of your model. You can also use Lip Sync and configure your own phoneme profil. You can also record your expression directly on live and use it after.
- You can Record video with your own expression record on Live Stream or import them. You have a timeline to place expression where you want to play them and record them to have a video of your different expressions.
- You can use Wizard of Oz. If you are not familiar with wizard of oz experiments, please follow this [link](https://en.wikipedia.org/wiki/Wizard_of_Oz_experiment). You create a server and you connect to it. You can choose what expression you want to play. The expression can be import or can record on live.

The Record and the WoZ are in progress. In Live Stream, Live, record and lip sync are usable. You can choose audio input device and also enable and disable lip sync but you can't add, modify or delete phomene.

In the futur, we want to improve the toolkit's Record and WoZ, with a better stability for the avatar. We also want to be able to add your own avatar, have a API systeme to make using your model with the avatar more functional and practical and respond to the need of the scientific community.

## How To Use :
First, you have to start the ACT Server :

![](https://github.com/Arthur-P0/ACT/blob/ACT_experimental/Unity%20Side/README_Img/GIF_Start_Serveur.gif)

After that, you lauch the unity build :

PICTURE OF ACT UNITY START 

Here you have 4 choice :

- Configuration :
  You can configurate your avatar here :
  PICTURE OF CONFIGURATION
  In the first part, you can change neck and head correction, transition duration and if you want the total expression or not.
  In the second part, you can add, edit and delete your own action. You have to import a CSV with all your action unit.


- Record : Here, you can record your own action previously update on Configuration : PICTURE OF RECORD.
  You have a control pannel, where all action are register. You can drag and drop an action in the queue to create your own record.
  You can adjust head and neck rotation to have the best possible record.


- Wizard Of Oz : Here, you can create a room and connect to it to use expression record before.PICTURE OF WoZ.
  All the expression record are on your left, you can press on it to see the expression in action.


- LiveStreaming : !!Warning!! Before launch live Streaming, you have to start Openface so that server can have the data send by openface to LiveStream your expression.
PICTURE OF LIVESTREAMING. You can record your expression in realtime, and you can change the parameter of lips sync like phoneme, your mic or enable diseable your mic.

## How does it work 

We use openface, who sends these action unit to our server. In the server, the action unit are transform to BlendShape expression. One action unit is linked to several BlendShape.
We take the value of the action unit and we adjust it to work on Unity BlenShape. After this conversion, we send a Json File with all BlendShape name and value and we apply it to the avatar.
