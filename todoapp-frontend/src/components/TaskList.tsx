import React, { useState, useEffect } from "react";
import { createTask, getMyTasks } from "../api/tasks";
import { useAuthStore } from "../store/authStore";

const TaskList: React.FC = () => {
  const [tasks, setTasks] = useState<any[]>([]);
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const { token } = useAuthStore();

  const fetchTasks = async () => {
    if (!token) return;
    try {
      const data = await getMyTasks(token);
      setTasks(data);
    } catch (err) {
      console.error("Failed to fetch tasks", err);
    }
  };

  useEffect(() => {
    fetchTasks();
  }, [token]);

  const handleCreateTask = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!token) return;
    try {
      await createTask(title, description || null, token);
      setTitle("");
      setDescription("");
      fetchTasks(); // Opdater listen
    } catch (err) {
      console.error("Failed to create task", err);
    }
  };

  return (
    <div>
      <h2>My Tasks</h2>
      <form onSubmit={handleCreateTask}>
        <div>
          <label>Title:</label>
          <input
            type="text"
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            required
          />
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
            {task.title} {task.description && `(${task.description})`} -{" "}
            {task.isCompleted ? "Completed" : "Not Completed"}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default TaskList;