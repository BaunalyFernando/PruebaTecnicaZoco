import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Login from './views/Login/login';
import Home from './views/home/home';
import './App.css';

function App() {
  return (
    <BrowserRouter>
      <Routes>    
        <Route path="/login" element={<Login />} />
        <Route path="/home" element={<Home />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
