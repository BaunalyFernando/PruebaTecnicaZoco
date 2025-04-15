import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axios from 'axios';
import styles from '../home/Home.module.css';
import Navbar from '../../components/Navbar';

const EditAddress = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const token = sessionStorage.getItem('token');
  const userId = sessionStorage.getItem('userId');
  const [address, setAddress] = useState({
    calle: '',
    numero: '',
    ciudad: '',
    userId: userId || '',
  });
  

  useEffect(() => {
    if (!token) {
      navigate('/login');
      return;
    }
  
    console.log('ID de dirección a editar:', id);
    
    const config = { headers: { Authorization: `Bearer ${token}` } };
    
    axios.get(`${import.meta.env.VITE_API_BASE_URL}/api/Addresses/${userId}`, config)
      .then(res => {
        console.log('Todas las direcciones:', res.data);
        
        if (res.data && res.data.$values) {
          const addressFound = res.data.$values.find(addr => addr.id === Number(id));
          
          if (addressFound) {
            setAddress({
              calle: addressFound.calle || '',
              numero: addressFound.numero || '',
              ciudad: addressFound.ciudad || '',
              userId: addressFound.userId || Number(userId)
            });
          } else {
            console.error('No se encontró la dirección con ID:', id);
          }
        } else {
          console.error('No se encontraron datos de direcciones');
        }
      })
      .catch(err => {
        console.error('Error al obtener direcciones:', err);
      });
  }, [id, userId]);

  const handleChange = e => {
    const { name, value } = e.target;
    setAddress(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = e => {
    e.preventDefault();
    const config = { headers: { Authorization: `Bearer ${token}` } };
  
    const payload = {
      id: Number(id),        
      userId: Number(userId), 
      ...address
    };
  
    axios.put(`${import.meta.env.VITE_API_BASE_URL}/api/addresses`, payload, config)
      .then(() => navigate('/profile'))
      .catch(err => console.error(err));
  };
  

  return (
    <div className={styles.container}>
      <Navbar />
      <div className={styles.content}>
        <h1>Editar Dirección</h1>
        <form onSubmit={handleSubmit} className={styles.form}>
          <label>
            Calle:
            <input type="text" name="calle" value={address.calle} onChange={handleChange} required />
          </label>
          <label>
            Número:
            <input type="text" name="numero" value={address.numero} onChange={handleChange} required />
          </label>
          <label>
            Ciudad:
            <input type="text" name="ciudad" value={address.ciudad} onChange={handleChange} required />
          </label>
          <button type="submit" className={styles.saveButton}>Guardar Cambios</button>
        </form>
      </div>
    </div>
  );
};

export default EditAddress;
