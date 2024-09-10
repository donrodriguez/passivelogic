# PassiveLogic Physics Simulator

This repository contains the PassiveLogic Physics Simulator: A Vue.js web app and a .NET API. Follow the steps below to set up and run the simulator using Docker.

## Installation and Setup

Follow these steps to get the application running:

### Step 1: Install Docker Desktop

If you haven't already installed Docker Desktop, [download and install Docker Desktop](https://docs.docker.com/desktop/install/mac-install/) for your operating system (Windows, macOS, or Linux).

### Step 2: Clone the Repository

Clone this GitHub repository to your local machine.

```bash
git clone https://github.com/donrodriguez/passivelogic.git
```
Then navigate to the following directory:

```bash
cd /PassiveLogic/PhysicsSimulator
```

### Step 3: Run the Application

Once inside the `PhysicsSimulator` directory where `docker-compose.yaml` is located, run the following command to start the application using Docker Compose:

```bash
docker-compose up -d
```

- The `-d` flag runs the containers in detached mode, which means the containers will run in the background.

### Step 4: Access the Application

Once Docker Compose has started the containers, you can access the application through your browser by visiting the appropriate URL. Ensure you are using the correct port specified in the `docker-compose.yml` file.

### Step 5: Stopping the Containers

To stop the running containers, you can use the following command:

```bash
docker-compose down
```

This will stop and remove the containers defined in the Docker Compose file.

## License

This project is licensed under the MIT License.
