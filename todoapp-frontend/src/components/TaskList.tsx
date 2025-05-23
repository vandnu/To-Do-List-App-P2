import React, { useState, useEffect } from "react";
import axios, { AxiosError } from "axios";
import { useAuthStore } from "../store/authStore";
import { useNavigate } from "react-router-dom";

interface Task {
  id: number;
  title: string;
  description?: string;
  isCompleted: boolean;
  userId: number;
}

const TaskList: React.FC = () => {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const { token, role, userId, logout } = useAuthStore(); // Hent userId fra store
  const navigate = useNavigate();

  // Valider at userId er der
  if (!userId) {
    console.error("UserId is not available. Logging out...");
    logout();
    navigate("/login");
    return null;
  }

  const fetchTasks = async () => {
    if (!token) return;
    try {
      const url = role === "Admin" ? "/api/ToDoTasks/all-tasks" : "/api/ToDoTasks/my-tasks";
      const response = await axios.get(`https://localhost:7100${url}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      setTasks(response.data);
    } catch (err) {
      console.error("Failed to fetch tasks", err);
    }
  };

  const updateTaskCompletion = async (taskId: number, isCompleted: boolean) => {
    if (!token) return;
    const taskToUpdate = tasks.find((t) => t.id === taskId);
    if (!taskToUpdate) return;
  
    //console.log("Attempting to update task", taskId, "to isCompleted:", isCompleted);
    try {
      const response = await axios.patch(
        `https://localhost:7100/api/ToDoTasks/${taskId}/complete`,
        isCompleted, // Send boolean som JSON
        { 
          headers: { 
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json" // Tilføj Content-Type
          }
        }
      );
      console.log("Update successful, response:", response.data);
      setTasks(tasks.map(task => task.id === taskId ? { ...task, isCompleted } : task));
      fetchTasks();
    } catch (err: unknown) {
      if (err instanceof AxiosError) {
        console.error("Failed to update task", err.response?.status, err.response?.data);
      } else {
        console.error("Unexpected error:", err);
      }
      setTasks(tasks.map(task => task.id === taskId ? { ...task, isCompleted: !isCompleted } : task));
    }
  };

  const deleteTask = async (taskId: number) => {
    if (!token) return;
    if (role !== "Admin") {
      console.error("Kun admin kan slette opgaver.");
      return;
    }
    try {
      await axios.delete(`https://localhost:7100/api/ToDoTasks/${taskId}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      console.log("Task deleted:", taskId);
      fetchTasks();
    } catch (err) {
      console.error("Failed to delete task", err);
    }
  };

  const handleCreateTask = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!token) return;
    try {
      await axios.post(
        "https://localhost:7100/api/ToDoTasks",
        { title, description: description || null, isCompleted: false },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      setTitle("");
      setDescription("");
      fetchTasks();
    } catch (err) {
      console.error("Failed to create task", err);
    }
  };

  useEffect(() => {
    fetchTasks();
  }, [token, role]);

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  return (
    <div>
      <div>
        <h2>{role === "Admin" ? "Alle opgaver" : "Mine opgaver"}</h2>
        <button onClick={handleLogout}>Log ud</button>
      </div>
      <form onSubmit={handleCreateTask}>
        <div>
          <label>Title:</label>
          <input type="text" value={title} onChange={(e) => setTitle(e.target.value)} required />
        </div>
        <div>
          <label>Description (optional):</label>
          <input
            type="text"
            value={description}
            onChange={(e) => setDescription(e.target.value)}
          />
        </div>
        <button type="submit">Create Task</button>
      </form>
      <ul>
        {tasks.map((task) => (
          <li key={task.id}>
            <h3>{task.title}</h3>
            <p>{task.description || "Ingen beskrivelse"}</p>
            <label>
              <input
                type="checkbox"
                checked={task.isCompleted}
                onChange={(e) => updateTaskCompletion(task.id, e.target.checked)}
              />
              {task.isCompleted ? "Completed" : "Not Completed"}
            </label>
            {role === "Admin" && (
              <button
                onClick={() => deleteTask(task.id)}
                style={{ marginLeft: "10px", backgroundColor: "red", color: "white", padding: "5px" }}
              >
                Slet
              </button>
            )}
          </li>
        ))}
      </ul>
      </div>
    );
  };

export default TaskList;