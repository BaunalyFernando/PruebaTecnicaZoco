import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axios from 'axios';
import Navbar from '../../components/Navbar';

const EditStudy = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [study, setStudy] = useState({ nombre: '', descripcion: '' });
  const token = sessionStorage.getItem('token');
  const userId = sessionStorage.getItem('userId');

  useEffect(() => {
    if (!token) {
      navigate('/login');
      return;
    }

    console.log('ID de estudio a editar:', id);
    
    const config = { headers: { Authorization: `Bearer ${token}` } };
    
    axios.get(`${import.meta.env.VITE_API_BASE_URL}/api/Studies/${userId}`, config)
      .then(res => {
        console.log('Todos los estudios:', res.data);
        
        if (res.data && res.data.$values) {
          const studyFound = res.data.$values.find(s => s.id === Number(id));
          
          if (studyFound) {
            setStudy({
              nombre: studyFound.nombre || '',
              descripcion: studyFound.descripcion || '',
            });
          } else {
            console.error('No se encontró el estudio con ID:', id);
          }
        } else {
          console.error('No se encontraron datos de estudios');
        }
      })
      .catch(err => {
        console.error('Error al obtener estudios:', err);
      });
  }, [id, userId]);

  const handleChange = (e) => {
    setStudy({ ...study, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const config = { headers: { Authorization: `Bearer ${token}` } };

    const payload = {
      id: Number(id),
      userId: Number(userId),
      ...study
    };

    axios.put(`${import.meta.env.VITE_API_BASE_URL}/api/Studies`, payload, config)
      .then(() => navigate('/profile')) 
      .catch(err => console.error(err));
  };

  return (
    <form onSubmit={handleSubmit}>
      <Navbar />
      <h2>Editar Estudio</h2>
      <label>Nombre:</label>
      <input name="nombre" value={study.nombre} onChange={handleChange} />
      <label>Descripción:</label>
      <input name="descripcion" value={study.descripcion} onChange={handleChange} />
      <button type="submit">Guardar</button>
    </form>
  );
};

export default EditStudy;