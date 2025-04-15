import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axios from 'axios';
import styles from '../home/Home.module.css';
import Navbar from '../../components/Navbar';

const EditUser = () => {
  const { id } = useParams();
  const [user, setUser] = useState(null);
  const [nombre, setNombre] = useState('');
  const [apellido, setApellido] = useState('');
  const [email, setEmail] = useState('');
  const [role, setRole] = useState(0);
  const navigate = useNavigate();
  const token = sessionStorage.getItem('token');

  useEffect(() => {
    if (!token) {
      navigate('/login');
      return;
    }

    console.log('ID de usuario a editar:', id);
    
    const config = { headers: { Authorization: `Bearer ${token}` } };

    axios.get(`${import.meta.env.VITE_API_BASE_URL}/api/users`, config)
      .then(res => {
        console.log('Respuesta de la API (todos los usuarios):', res.data);
        
        if (res.data && res.data.$values) {
          const userFound = res.data.$values.find(u => u.id === Number(id));
          
          if (userFound) {
            setUser(userFound);
            setNombre(userFound.nombre || '');
            setApellido(userFound.apellido || '');
            setEmail(userFound.email || '');
            setRole(userFound.role || 0);
          } else {
            console.error('No se encontró el usuario con ID:', id);
            navigate('/users');
          }
        } else {
          console.error('No se encontraron datos de usuarios');
          navigate('/users');
        }
      })
      .catch(err => {
        console.error('Error al obtener usuarios:', err);
        navigate('/users');
      });
  }, [id]);

  const handleSubmit = (e) => {
    e.preventDefault();

    const config = {
      headers: {
        Authorization: `Bearer ${token}`,
        'Content-Type': 'application/json'
      }
    };

    const payload = {
      id: parseInt(id),
      nombre,
      apellido,
      email,
      role,
      password: user.password 
    };

    axios.put(`${import.meta.env.VITE_API_BASE_URL}/api/Users/UpdateUserByAdmin`, payload, config)
      .then(() => {
        console.log('Usuario actualizado correctamente');
        navigate(`/users/${id}`);
      })
      .catch(err => {
        console.error('Error al actualizar usuario:', err);
      });
  };

  if (!user) return <div className={styles.container}>Cargando...</div>;

  return (
    <div className={styles.container}>
      <Navbar />
      <div className={styles.content}>
        <h2>Editar Usuario</h2>
        <form onSubmit={handleSubmit} className={styles.form}>
          <label>Nombre:</label>
          <input value={nombre} onChange={(e) => setNombre(e.target.value)} required />

          <label>Apellido:</label>
          <input value={apellido} onChange={(e) => setApellido(e.target.value)} required />

          <label>Email:</label>
          <input type="email" value={email} onChange={(e) => setEmail(e.target.value)} required />

          <label>Rol:</label>
          <select value={role} onChange={(e) => setRole(parseInt(e.target.value))}>
            <option value={0}>Administrador</option>
            <option value={1}>Usuario</option>
          </select>

          <button type="submit" className={styles.saveButton}>Guardar cambios</button>
        </form>
      </div>
    </div>
  );
};

export default EditUser;