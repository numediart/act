# Avatar Controller Toolkit

<div align="center">
  <p align="center">
    The Avatar Controller Toolkit (ACT) is a tool developped to enhance the visualisation of any expression model which would be performed by an agent, Lou, we have created using Blender.
    The ACT project uses the power of Unity and Blender, mixed in a tool made for the users. The advantage of our project is you have no constraints for the choice of model you want to use. You can plug your own model on the server and use the avatar.
    <br />
    <br />
    <a href="https://github.com/numediart/ACT/wiki">View Wiki</a>
    ·
    <a href="https://github.com/numediart/ACT/issues">Report Bug</a>
  </p>
</div>

<!-- ABOUT THE PROJECT -->
## About The Project

Here's a preview of what you will find in that repository. To make it simple, we wanted to create an avatar in a virtual environment that will be able to replay expressions form yourself or your AI Model.

In doing so, we ended creating a video exporter, where you can create a sequence of expression you want the avatar to perform and export it as a MP4 file.

We have also implemented Wizard of Oz experiment interface that allows you to perform wizard of oz experiment easely, using a server and a build of the project. If you are not familiar with wizard of oz experiments, please follow this [link](https://en.wikipedia.org/wiki/Wizard_of_Oz_experiment).

Finally, we have implemented live streaming experiment, where you can use your model (here openface) with your webcam and have the movements of the head, mouth, etc. transmitted live to the avatar.

In the future, we want to improve the toolkit's modularity, where you will be able to add your own avatar, use different types of expression, not only .csv, and respond to the need of the scientific community.

This project has highly been inspired by an older project we made in Godoot Engine, find the link [here](https://github.com/numediart/ReVA-toolkit).

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- GETTING STARTED -->
## Getting Started

1. Download the project
2. In the folder "Server Side", open the solution "act_server.sln"
3. Start the solution
4. Open the folder "Unity Side" on Unity
5. Go to the scene "Ver 1.0 - Menu", in the folder "Scene"
6. Launch the unity project, Here you have Four Choice :
   - Configuration : You can configurate the setting of record and add csv file with your expression
   - Record : Here you can create a video of a sequence of expression you have made
   - Wizard Of Oz : In this part, You create a room to control the avatar with expression you made
   - Live Stream : With openface or your model, you can control avatar, use lipsync and record our expression
> [!WARNING]
> To use livestream, check the video :
> 
> ![](https://github.com/Arthur-P0/ACT/blob/ACT_experimental/Readme_Vid/Tutorial-LiveStream.gif)

<!-- HOW ? -->
## How does it work ?

For LiveStreaming : Actually, it's support one model [Openface](https://github.com/numediart/openface_act) .We use it to get action unit. We formatted action unit into json and sent to the server. The server transform it to blendshape which can be read by Unity to move the avatar.

For Record : Add your expression in .csv or .json in the configuration menu. Unity read the file and process it to the avatar to create a record of your different expression

For Wizard of Oz : You have one agent, who is the admin, and a client. The agent send expression to the client through the server.

<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you want to contribute to the project, please follow the steps below :

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- TODO -->
## To Do
* Improving mediapipe
* Improve server connection reliability
* Improve the current avatar add blendshapes
* Debugging the Wizard of Oz and the Record (lag on the avatar)
* Plugin system for modding/adding features
* Add error handling on the client and server
* Make mediapipe and openface connections more reliable
* Server unit test
* Optimise data formatting

<!-- LICENSE -->
## License

See `LICENSE` for more information.

<!-- CONTACT -->
## Contact

- Kevin El Haddad - ACT Creator
- [Armand DEFFRENNES](https://github.com/JambonPasFrais) - armand.deffrennes@student.junia.com - ACT Dev
- [Pierre-Luc MILLET](https://github.com/Pierre-LucM)- pierre-luc.millet@student.junia.com - ACT Dev
- [Arthur PINEAU](https://github.com/Arthur-P0) - arthur.pineau@student.junia.com - ACT Dev


Project Link: [Git Repo](https://github.com/numediart/ACT)

<p align="right">(<a href="#readme-top">back to top</a>)</p>
