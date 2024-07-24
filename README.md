# Avatar Controller Toolkit

<div align="center">
  <p align="center">
    The Avatar Controller Toolkit (ACT) is developed to enhance the visualization of any expression model performed by an agent, Lou, created using Blender.
    ACT leverages the power of Unity and Blender, offering three key features: <b>Live Streaming</b>, <b>Recording</b>, and <b>Wizard of Oz</b>. The primary advantage of our project is its flexibility; you can use any model with the server to animate the avatar.
    <br />
    <br />
    <a href="https://github.com/numediart/ACT/tree/ACT_experimental/UnitySide">View Unity Side Readme</a>
    ·
    <a href="https://github.com/numediart/ACT/tree/ACT_experimental/ServerSide">View Server Side Readme</a>
    .
    <a href="https://github.com/numediart/ACT/issues">Report Bug</a>
  </p>
</div>

## About The Project

This repository aims to create an avatar in a virtual environment capable of replaying expressions from either yourself or your AI model. We have developed a video exporter that allows users to create a sequence of expressions for the avatar and export it as an MP4 file.

Additionally, we have implemented a Wizard of Oz experiment interface, facilitating the execution of such experiments using a server and a project build. For those unfamiliar with Wizard of Oz experiments, more information can be found [here](https://en.wikipedia.org/wiki/Wizard_of_Oz_experiment).

Lastly, the live streaming feature enables real-time control of the avatar using models such as OpenFace with a webcam, transmitting head, mouth, and other movements to the avatar live.

Future improvements aim to enhance the toolkit's modularity, allowing users to add their own avatars, utilize various expression types beyond .csv, and meet the scientific community's needs.

This project is inspired by an older project developed in the Godot Engine, which can be found [here](https://github.com/numediart/ReVA-toolkit).

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Getting Started

1. Download the project.
2. In the "Server Side" folder, open the solution "act_server.sln".
3. Start the solution.
4. Open the "Unity Side" folder in Unity.
5. Navigate to the "Ver 1.0 - Menu" scene in the "Scene" folder.
6. Launch the Unity project. You will have four options:
   - **Configuration:** Configure the recording settings and add .csv or .json files with your expressions.
   - **Record:** Create a video of a sequence of expressions.
   - **Wizard of Oz:** Create a room to control the avatar with predefined expressions.
   - **Live Stream:** Use OpenFace or your model to control an avatar, utilizing lip sync and recording your expressions.
     


> [!WARNING]
> To use livestream, check the video :
> 
> ![](https://github.com/numediart/ACT/blob/ACT_experimental/Readme_Vid/Tutorial-LiveStream.gif)
> 
>  Begin by starting the server. The server will handle the reception and distribution of blendshape data.
> 
>  Open the Unity client and create a livestream room.
> 
> In the server console, note the room ID. When prompted by the Mediapipe script or in config.xml for Openface, enter this room ID to establish the connection.

## How Does It Work?

### Live Streaming

Currently, the live streaming feature supports two models, [OpenFace](https://github.com/numediart/openface_act) and [Mediapipe](https://github.com/numediart/mediapipe_act). Openface captures action units using the webcam, formats them into JSON, and sends them to the server. The server converts these units into blendshapes readable by Unity to animate the avatar.
Mediapipe captures the face landmarks using the webcam, formats them into JSON, and sends formatted blendshapes to the server. The server then sends these blendshapes to Unity to animate the avatar.
### Record

Add your expressions in .csv or .json format in the configuration menu. Unity reads the file and processes it to create a record of your various expressions.

### Wizard of Oz

In this mode, an agent (admin) sends expressions to a client through the server, allowing for controlled experiments.

## Contributing

Contributions are what make the open-source community such a great place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

To contribute to the project, follow these steps:

1. Fork the Project.
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`).
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`).
4. Push to the Branch (`git push origin feature/AmazingFeature`).
5. Open a Pull Request.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## To Do

* Create an online server.
* Debug recording (skip the second expression, create a UI for the record video path, add recording to the server).
* Improve Mediapipe integration.(mediapipe implementation for act may need improvement, especially the blendshape formatting part)
* Enhance server connection reliability.
* Improve the current avatar and add blendshapes.
* Debug Wizard of Oz and Record features (reduce lag).
* Add error handling on the client and server.
* Ensure reliable connections for Mediapipe and OpenFace.
* Implement server unit tests.
* Optimize data formatting.
* Handle client disconnections. (the current version can't handle self disconnection)
* Manage client reconnections.
* Transmit room ID to OpenFace or other facial recognition software using WebSockets.
* Add error handling and error events.
* Refacto Unity Side architecture for a MVC (Model View Controller) architecture (implement a Factory pattern builder to create new room)
* Interrupt system for Wizard of Oz (interrupt the current expression to play a new one)

## Bugs
### Livestream:

- Connection with the server may randomly disconnect.
- OpenFace or Mediapipe can lose connection with the server.
- Review the multipliers for blendshapes to ensure they appear more realistic.
- The lipsync settings do not cover the entire game view when opened.
- There are occasional latency issues between the server and the livestream, causing the avatar to lag by a few seconds (potentially due to data queuing?).

### Wizard of Oz:

- Connection with the server may randomly disconnect (both client and admin).
- JSON format is not supported. Please use CSV file instead
- Certain expressions are skipped (notably the second one).

### Record:

- Video recording functionality is unreliable; the click sometimes does not work.
- Certain expressions are skipped (notably the second one).
- (Feature request) Prompt for the video save path at the start or end of the recording.

## License

See `LICENSE` for more information.

## Contact

- Kevin El Haddad - PI
- [Armand DEFFRENNES](https://github.com/JambonPasFrais) - armand.deffrennes@student.junia.com - ACT Developer
- [Pierre-Luc MILLET](https://github.com/Pierre-LucM) - pierre-luc.millet@student.junia.com - ACT Developer
- [Arthur PINEAU](https://github.com/Arthur-P0) - arthur.pineau@student.junia.com - ACT Developer

Project Link: [Git Repo](https://github.com/numediart/ACT)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

---

