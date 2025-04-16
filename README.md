# buho, Multiplayer Matchmaking System Implementation.

Welcome to the **buho Multiplayer Interview Project**!

This Unity project is set up for candidates to demonstrate their skills in **multiplayer networking** using Unity’s **Relay** and **Netcode for GameObjects**. You will build a basic **player matching system**, and transition players between scenes using a provided screen transition package.

---

## 📁 Repository Structure

> ⚠️ **Important Note on Branches**

- `master` branch — **Empty Unity project** (barebones setup)
- `develop` branch — Unity project with **Unity Services integration (Relay, Netcode)**

👉 **Always work from the `develop` branch** when starting your task.

```bash
# Example:
git checkout develop
```

---

## 🔧 Project Description

This Unity project includes:

- Unity Services package (Relay + Netcode for GameObjects)
- A **pre-integrated screen transition package** (to switch scenes across the network)
- No UI/UX or gameplay logic — that’s your job!

---

## 🎯 Your Task

Implement a working **multiplayer matching system** using Unity Relay & Netcode. When two players are connected, they should be transitioned from the **Menu** scene to the **Game** scene using the screen transition system.

### ✔️ Required Features

1. **Scene Setup**

   - Create two scenes:
     - `Menu` — Where player initiates/join matchmaking
     - `Game` — Where both players are synced after connection

2. **Matching System**

   - Use **Unity Relay + Netcode** to allow two players to connect.
   - UI appearance is not important — just show that the connection works.

3. **Screen Transition**

   - Use this package (do not customize):
     - 📦 [network-pack](https://github.com/buho-Game/network-pack)
   - After a successful match, trigger a screen transition from `Menu` to `Game` **for both players** using the above package.

4. **Code Quality**
   - Use clear, modular, and well-organized C# code.
   - Use Unity naming conventions and keep code readable.

---

## 🌟 Bonus Objectives

You’ll stand out if you implement any of the following:

- 🕹️ **Game interaction:**  
  Sync a simple action in the `Game` scene using `ServerRpc` or `ClientRpc`.  
  Example: A button that sends a message to both clients.

- 🔁 **Reconnect Handling:**  
  If one player disconnects, return to the `Menu` screen.  
  Let players reconnect afterward.

- 📦 **Session Reset:**  
  Implement server shutdown logic and gracefully handle cleanup (e.g., return to `Menu` on both sides).

---

## 🧪 How We Will Review

We'll evaluate your submission based on:

| Criteria                 | Notes                                                  |
| ------------------------ | ------------------------------------------------------ |
| ✅ Netcode & Relay Usage | Are players able to connect via Unity Services?        |
| ✅ Scene Transition      | Is the provided transition system properly integrated? |
| ✅ Code Style            | Clean, modular, and consistent C# code                 |
| 🔁 Bonus Features        | Game logic, reconnection, or message syncs             |

---

## 🧰 Setup Instructions

```bash
# Clone your fork and switch to develop branch
git clone <your-fork-url>
cd <repo-name>
git checkout develop

# Open the project in Unity
# Unity version should match the one used in 'develop' branch (usually latest LTS)
```

---

## 🔗 Required Package

Make sure to use the screen transition system from:

📦 **Screen Package:**  
https://github.com/buho-Game/network-pack

> **Customization of the screen transition method is not allowed.**

---

## 📤 Submission Instructions

Once you’re done:

```bash
# Commit your work
git add .
git commit -m "Implemented matchmaking system + scene transition"

# Push your changes
git push origin <your-feature-branch>

# Go to GitHub and create a Pull Request back to this repository
```

> ✅ **Submit a Pull Request targeting the `develop` branch** of this repository.  
> Include a short summary describing your implementation and any bonus features completed.

---

## 🙌 Good Luck!

This project is part of the **Braverse Multiplayer Initiative**.  
We’re looking forward to seeing your ideas in action. Show us how you approach multiplayer systems in Unity — and have fun doing it!

Happy coding! 💻🎮
