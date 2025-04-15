import React, { useEffect, useState } from 'react';
import axios from 'axios';
import styles from '../home/Home.module.css';
import Navbar from '../../components/Navbar';
import { useNavigate } from 'react-router-dom';

const Users = () => {
  const navigate = useNavigate();
  const [users, setUsers] = useState([]);
  const token = sessionStorage.getItem('token');

  useEffect(() => {
    if (!token) {
      window.location.href = '/login';
      return;
    }

    const config = { headers: { Authorization: `Bearer ${token}` } };

    axios
      .get(`${import.meta.env.VITE_API_BASE_URL}/api/users`, config)
      .then(res => {
        setUsers(res.data.$values);
      })
      .catch(() => {
        window.location.href = '/login';
      });
  }, []);

  return (
    <div className={styles.container}>
      <Navbar />

      <div className={styles.content}>
        <div className={styles.section}>
          <h2>Lista de Usuarios</h2>
          <button onClick={() => navigate('/registerAdmin')} className={styles.createUserBtn}>
            Crear Nuevo Usuario
          </button>
          {users.length === 0 ? (
            <p>No hay usuarios registrados.</p>
          ) : (
            <ul className={styles.list}>
              {users.map((user) => (
                <li key={user.id} className={styles.listItem}>
                  <strong>{user.nombre}</strong> ({user.email}) - {user.role === 0 ? 'Administrador' : 'Usuario'}
                  <button onClick={() => navigate(`/users/${user.id}`)}>Ver Detalle</button>
                </li>
              ))}
            </ul>
          )}
        </div>
      </div>
    </div>
  );
};

export default Users;
