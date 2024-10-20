"use client"; 
import React, { useState, useEffect } from 'react';
import Pusher from 'pusher-js';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min';
import 'bootstrap-icons/font/bootstrap-icons.css';
import './ChatApp.css';

const API_URL = 'https://localhost:44375/api/chat';

const ChatApp = () => {
  const [messages, setMessages] = useState([]);
  const [username, setUsername] = useState('');
  const [message, setMessage] = useState('');
  const [isTyping, setIsTyping] = useState(false);
  const [isChatVisible, setChatVisible] = useState(false);
  const [isSendButtonEnabled, setSendButtonEnabled] = useState(false);

  useEffect(() => {
    if (isChatVisible) {
      // Fetch existing messages
      fetch(`${API_URL}/get-messages`)
        .then(response => response.json())
        .then(data => setMessages(data));

      const pusher = new Pusher('93b856e799774453df5a', {
        cluster: 'eu',
      });

      const channel = pusher.subscribe('chat-room');

      channel.bind('new-message', (data) => {
        setMessages((prevMessages) => [...prevMessages, data]);
      });

      channel.bind('message-updated', (data) => {
        setMessages((prevMessages) =>
          prevMessages.map(msg =>
            msg.id === data.messageId ? { ...msg, text: data.text } : msg
          )
        );
      });

      channel.bind('message-deleted', (data) => {
        setMessages((prevMessages) =>
          prevMessages.filter(msg => msg.id !== data.messageId)
        );
      });

      channel.bind('message-reacted', (data) => {
        setMessages((prevMessages) =>
          prevMessages.map(msg =>
            msg.id === data.messageId
              ? {
                  ...msg,
                  Reactions: [
                    ...(msg.Reactions || []),
                    { ReactionType: data.reaction, Username: data.username, numOfReactions: data.numOfReactions }
                  ]
                }
              : msg
          )
        );
      });

      channel.bind('user-typing', (data) => {
        if (data.username !== username) {
          setIsTyping(true);
          setTimeout(() => setIsTyping(false), 2000);
        }
      });

      channel.bind('user-stop-typing', () => {
        setIsTyping(false);
      });

      return () => {
        channel.unbind_all();
        channel.unsubscribe();
      };
    }
  }, [isChatVisible, username]);

  const handleInputChange = (e) => {
    const inputText = e.target.value;
    setMessage(inputText);
    setSendButtonEnabled(inputText.trim().length > 0);

    if (inputText.trim().length > 0) {
      fetch(`${API_URL}/start-typing`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ username }),
      });
    } else {
      fetch(`${API_URL}/stop-typing`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ username }),
      });
    }
  };

  const sendMessage = async (e) => {
    e.preventDefault();
    if (message.trim()) {
      const response = await fetch(`${API_URL}/send-message`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ username, message }),
      });

      if (response.ok) {
        const newMessage = await response.json();
        setMessages([...messages, newMessage]);
        setMessage('');
        setSendButtonEnabled(false);
      } else {
        const errorResponse = await response.json();
        console.error('Error sending message:', errorResponse);
      }
    }
  };

  const handleUserNameChange = (e) => {
    setUsername(e.target.value);
  };

  const handleUserNameSubmit = (e) => {
    e.preventDefault();
    if (username.trim().length > 0) {
      setChatVisible(true);
    }
  };

  const handleEdit = async (msg) => {
    const newText = prompt("Edit your message:", msg.text);
    if (newText && newText.trim().length > 0) {
      const response = await fetch(`${API_URL}/update-message`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ id: msg.id, message: newText }),
      });

      if (!response.ok) {
        alert('Error updating message');
      }
    }
  };

  const handleDelete = async (msgId) => {
    const confirmDelete = window.confirm("Are you sure you want to delete this message?");
    if (confirmDelete) {
      const response = await fetch(`${API_URL}/delete-message/${msgId}`, {
        method: 'DELETE',
      });

      if (!response.ok) {
        alert('Error deleting message');
      }
    }
  };

  const handleReact = async (msgId, reaction) => {
    const response = await fetch(`${API_URL}/react-message`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({ messageId: msgId, reaction, username }),
    });

    if (!response.ok) {
      alert('Error adding reaction');
    }
  };

  return (
    <div className="chat-container mt-4">
      {!isChatVisible ? (
        <div className="username-input d-flex flex-column justify-content-center align-items-center" style={{ height: '70vh' }}>
          <h1 className="welcome-message">Welcome to the Chat Room</h1>
          <form onSubmit={handleUserNameSubmit} className="text-center mt-3">
            <input
              type="text"
              className="form-control chat-input mb-2"
              placeholder="Enter your name..."
              value={username}
              onChange={handleUserNameChange}
              style={{ width: '300px' }}
            />
            <button type="submit" className="btn btn-primary mt-2">Join</button>
          </form>
        </div>
      ) : (
        <div>
          <div className="chatroom-header">
            <h1 className="chat-title">Chat Room</h1>
          </div>

          <div className="chat-box mt-3 p-3 border rounded bg-light" style={{ height: '500px', overflowY: 'auto' }}>
            {messages.map((msg) => (
              <div key={msg.id} className={`d-flex align-items-center mb-3 ${msg.username === username ? 'justify-content-end' : ''}`}>
                <div className="d-flex align-items-center ms-4">
                  <img src="https://res.cloudinary.com/shehablotfallah/image/upload/v1729301005/avatar_t8mi9r.png" alt="User" className="user-image me-2" />
                  <div className={`message bg-white p-2 rounded ${msg.username === username ? 'my-message' : ''}`}>
                    <span className="fw-bold">{msg.username}</span>
                      <p>{msg.text}</p>
                    <div className="d-flex align-items-center justify-content-between">
                    <div className="reaction-display  d-option">
                        👍
                        <div className="reaction-tooltip">
                          {msg.Reactions && msg.Reactions.map((reaction, index) => (
                              <div key={index}>
                                <span className="d-block mb-1">
                                  {reaction.ReactionType} {reaction.Username}
                                </span>
                              </div>
                          ))}
                        </div>
                      </div>
                      <div className="message-time">{msg.timestamp}</div>
                    </div>
                  </div>
                </div>
                <div className="actions">
                    {/* خيارات التعديل والحذف */}
                    <span className="action-icon ms-2">⋮</span>
                  {msg.username === username && (
                      <div className="actions-options">
                        <span onClick={() => handleEdit(msg)} className="action btn btn-sm btn-outline-primary me-1">✏️</span>
                        <span onClick={() => handleDelete(msg.id)} className="action btn btn-sm btn-outline-danger me-1">🗑️</span>
                      </div>
                  )}
                </div>
                {/* ردود الفعل */}
                <div className="reactions">
                    {/* إضافة ردود الفعل */}
                    <span className="reaction-icon ms-2">🙂</span>
                    <div className="reactions-options">
                      <span onClick={() => handleReact(msg.id, '👍')} className="reaction btn btn-sm btn-outline-primary me-1" name="Like">👍</span>
                      <span onClick={() => handleReact(msg.id, '❤️')} className="reaction btn btn-sm btn-outline-danger me-1" name="Love">❤️</span>
                      <span onClick={() => handleReact(msg.id, '😂')} className="reaction btn btn-sm btn-outline-warning me-1" name="Laugh">😂</span>
                      <span onClick={() => handleReact(msg.id, '😮')} className="reaction btn btn-sm btn-outline-warning me-1" name="Wow">😮</span>
                      <span onClick={() => handleReact(msg.id, '😢')} className="reaction btn btn-sm btn-outline-warning me-1" name="Sad">😢</span>
                      <span onClick={() => handleReact(msg.id, '👏')} className="reaction btn btn-sm btn-outline-warning me-1" name="Clap">👏</span>
                    </div>
                  </div>
              </div>
            ))}
            {isTyping && (
              <div className="typing-indicator">
                <div className="typing-dot"></div>
                <div className="typing-dot"></div>
                <div className="typing-dot"></div>
              </div>
            )}
          </div>
          {/* إدخال الرسالة مع دمج الزر داخل input */}
          <form onSubmit={sendMessage} className="input-container mt-2">
            <div className="input-group">
              <input
                type="text"
                className="form-control message-input d-flex mt-0"
                placeholder="Type your message..."
                value={message}
                onChange={handleInputChange}
                style={{ height: '50px' }}/>
              <button 
                type="submit"
                className={`send-btn mt-4 ${!isSendButtonEnabled ? 'disabled' : ''}`}
                disabled={!isSendButtonEnabled}>
                <i className="bi bi-send"></i>
              </button>
            </div>
          </form>

        </div>
      )}
    </div>
  );
};

export default ChatApp;
