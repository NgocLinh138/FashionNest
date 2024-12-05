import React, { useState } from 'react';
import { login } from '../services/authService';
import Swal from 'sweetalert2';
import { useNavigate } from 'react-router-dom';

const LoginForm = ({ onLoginSuccess }) => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      const data = await login(email, password);
      localStorage.setItem('isLoggedIn', true);
      localStorage.setItem('user', JSON.stringify(data.user));

      Swal.fire({
        title: 'Đăng nhập thành công!',
        text: 'Chào mừng bạn quay lại.',
        icon: 'success',
        showConfirmButton: false,
        showCancelButton: false,
        background: '#fff',
        timer: 1000
      }).then(() => {
        onLoginSuccess(data);
      });

    } catch (error) {
      const errorMessage = error.response?.data?.message || 'Đăng nhập thất bại!';
      setError(errorMessage);

      Swal.fire({
        title: 'Lỗi!',
        text: errorMessage,
        icon: 'error',
        confirmButtonText: 'Thử lại'
      });
    }
  };

  return (
    <div className="space-y-4">
      <h2 className="text-3xl font-semibold text-gray-900 text-center mb-4">Đăng nhập</h2>
      {error && <p className="text-red-500 text-center">{error}</p>}
      <form onSubmit={handleLogin} className="space-y-4">
        <div>
          <label className="block text-sm font-medium text-gray-700">Email</label>
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
            className="mt-2 p-2 w-full border border-gray-300 rounded-lg shadow-sm focus:ring-2 focus:ring-blue-300 focus:outline-none transition-all duration-300"
          />
        </div>
        <div>
          <label className="block text-sm font-medium text-gray-700">Mật khẩu</label>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
            className="mt-2 p-2 w-full border border-gray-300 rounded-lg shadow-sm focus:ring-2 focus:ring-blue-300 focus:outline-none transition-all duration-300"
          />
        </div>
        <button
          type="submit"
          className="w-full bg-gradient-to-r from-pink-500 to-red-500 text-white py-3 px-4 rounded-lg shadow-lg hover:bg-gradient-to-l focus:outline-none transition-all duration-300"
        >
          Đăng nhập
        </button>
      </form>
      <div className="text-center mt-4">
        <p className="text-sm text-gray-600">
          Chưa có tài khoản?{' '}
          <button
            onClick={() => navigate('/register')}
            className="text-pink-500 font-semibold hover:text-pink-700 focus:outline-none"
          >
            Đăng ký ngay
          </button>
        </p>
      </div>
    </div>
  );
};

export default LoginForm;
