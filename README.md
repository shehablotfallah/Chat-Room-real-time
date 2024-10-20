
# **Real-Time Chat Room Project**

## **Overview**
This is a real-time chat application that allows users to send messages, update them, and react to them instantly. It is built with **ASP.NET Core API** for the backend and **Next.js** for the frontend. The app supports real-time communication using **PusherServer** for live message updates and reactions.
### Welcome Chat Screenshot
![Chat Room Preview](https://github.com/user-attachments/assets/6bbd85fa-1670-4bcd-a2b5-0280340e103c)
### Conversation in Progress
![Chat Room Preview UI](https://github.com/user-attachments/assets/cce46f08-dc5b-4abb-97b7-6a6aa587c80a)
### User Typing and Reactions
![Chat Room Preview Typing](https://github.com/user-attachments/assets/ccda33b3-1d35-47c2-8422-8225e5fbfb1f)



## **Technologies Used**
### **Backend**:
- **ASP.NET Core API** Version 8
- **Entity Framework Core** Version 8.0.10
- **SQLite Database**
- **PusherServer** for real-time communication (Version 5.0.0)
- **Swashbuckle.AspNetCore** for API documentation (Version 6.6.2)

### **Frontend**:
- **Next.js** Version 14.2.15
- **React** Version 18
- **Bootstrap** Version 5.3.3
- **Pusher.js** Version 8.4.0-rc2

## **Getting Started**

### **Prerequisites**
- **.NET SDK** (version 8 or later)
- **Node.js** (version v20.18.0 or later)
- **SQLite** for the database
- **Pusher** account for real-time communication

### **Backend Setup**

1. Clone the repository:
   ```bash
   https://github.com/shehablotfallah/Chat-Room-real-time.git
   cd Chat-Room-real-time
   ```

2. Install the necessary NuGet packages:
   ```bash
   dotnet restore
   ```

3. Apply database migrations:
   ```bash
   dotnet ef database update
   ```

4. Run the API:
   ```bash
   dotnet run
   ```

The API will be running on `https://localhost:44375`.

### **Frontend Setup**

1. Navigate to the frontend directory (if applicable):
   ```bash
   cd frontend-directory
   ```

2. Install the necessary dependencies:
   ```bash
   npm install
   ```

3. Start the Next.js server:
   ```bash
   npm run dev
   ```

The frontend will be running on `http://localhost:3000`.

## **API Endpoints**
Below are the available API endpoints for the chat functionality:

### API Endpoints Screenshot
![API Endpoints](https://github.com/user-attachments/assets/3288ed59-e5f1-4ba4-abf1-f166133843bc)

1. **Send a Message**  
   `POST /api/Chat/send-message`  
   **Request Body**:
   ```json
   {
     "id": 0,
     "username": "string",
     "message": "string"
   }
   ```

2. **Update a Message**  
   `PUT /api/Chat/update-message`  
   **Request Body**:
   ```json
   {
     "id": 0,
     "message": "string"
   }
   ```

3. **Delete a Message**  
   `DELETE /api/Chat/delete-message/{id}`

4. **React to a Message**  
   `POST /api/Chat/react-message`  
   **Request Body**:
   ```json
   {
     "messageId": 0,
     "reaction": "string",
     "username": "string"
   }
   ```

5. **Remove Reaction**  
   `POST /api/Chat/remove-reaction`  
   **Request Body**:
   ```json
   {
     "messageId": 0,
     "reaction": "string",
     "username": "string"
   }
   ```

6. **Start Typing Notification**  
   `POST /api/Chat/start-typing`  
   **Request Body**:
   ```json
   {
     "username": "string"
   }
   ```

7. **Stop Typing Notification**  
   `POST /api/Chat/stop-typing`  
   **Request Body**:
   ```json
   {
     "username": "string"
   }
   ```

8. **Get Messages**  
   `GET /api/Chat/get-messages`

## **Database Setup**
The application uses **SQLite** as the database. Make sure to apply migrations before running the project using the following command:
```bash
dotnet ef database update
```

## **Real-Time Communication with Pusher**
This project integrates **PusherServer** for real-time messaging. Make sure to configure your Pusher credentials in the project before running the application. You can obtain these from your [Pusher account](https://pusher.com/).

## **Frontend Features**
- **Bootstrap 5** is used for styling the frontend.
- The **Pusher.js** library is used to handle real-time events on the client-side.
- Users can:
  - Send messages
  - Update or delete messages
  - React to messages
  - See real-time typing indicators

## **Contributing**
If you'd like to contribute to this project, follow these steps:
1. Fork the repository.
2. Create a new branch (`git checkout -b feature-branch`).
3. Make your changes and commit them (`git commit -m "Add some feature"`).
4. Push the changes to your branch (`git push origin feature-branch`).
5. Open a pull request.

## **License**
This project is licensed under the [MIT License](LICENSE).
