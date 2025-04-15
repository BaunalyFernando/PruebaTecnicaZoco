import { Link, useNavigate } from 'react-router-dom';
import styles from '../views/home/Home.module.css';

const Navbar = () => {
  const role = sessionStorage.getItem('role');
  const navigate = useNavigate();

  const handleLogout = () => {
    sessionStorage.clear();
    navigate('/login');
  };

  return (
    <nav className={styles.navbar}>
      <div className={styles.navLeft}>
        <Link to="/home" className={styles.navLink}>Inicio</Link>
        {role === 'Admin' && <Link to="/users" className={styles.navLink}>Usuarios</Link>}
        <Link to="/profile" className={styles.navLink}>Perfil</Link>
      </div>
      <div className={styles.navRight}>
        <button onClick={handleLogout} className={styles.logoutButton}>Cerrar sesión</button>
      </div>
    </nav>
  );
};

export default Navbar;
