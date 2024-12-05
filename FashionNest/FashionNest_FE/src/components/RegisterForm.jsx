import React, { useState } from 'react';
import { register } from '../services/authService';
import { useNavigate } from 'react-router-dom';

const RegisterForm = ({ onRegisterSuccess }) => {
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [phoneNumber, setPhoneNumber] = useState('');
  const [address, setAddress] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleRegister = async (e) => {
    e.preventDefault();
    if (password !== confirmPassword) {
      setError('Mật khẩu và xác nhận mật khẩu không khớp!');
      return;
    }

    try {
      const data = await register(name, email, password, confirmPassword, phoneNumber, address);
      onRegisterSuccess(data);
    } catch (error) {
      const errorMessage = error.response?.data?.message || 'Đăng ký thất bại! Kiểm tra lại thông tin.';
      setError(errorMessage);
    }
  };

  return (
    <div className="space-y-4">
      <h2 className="text-3xl font-semibold text-gray-900 text-center mb-4">Đăng ký tài khoản</h2>
      {error && <p className="text-red-500 text-center">{error}</p>}
      <form onSubmit={handleRegister} className="space-y-4">
        <div>
          <label className="block text-sm font-medium text-gray-700">Họ và tên</label>
          <input
            type="text"
            value={name}
            onChange={(e) => setName(e.target.value)}
            required
            className="mt-2 p-2 w-full border border-gray-300 rounded-lg shadow-sm focus:ring-2 focus:ring-blue-300 focus:outline-none transition-all duration-300"
          />
        </div>
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
        <div>
          <label className="block text-sm font-medium text-gray-700">Xác nhận mật khẩu</label>
          <input
            type="password"
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
            required
            className="mt-2 p-2 w-full border border-gray-300 rounded-lg shadow-sm focus:ring-2 focus:ring-blue-300 focus:outline-none transition-all duration-300"
          />
        </div>
        <div>
          <label className="block text-sm font-medium text-gray-700">Số điện thoại</label>
          <input
            type="text"
            value={phoneNumber}
            onChange={(e) => setPhoneNumber(e.target.value)}
            required
            className="mt-2 p-2 w-full border border-gray-300 rounded-lg shadow-sm focus:ring-2 focus:ring-blue-300 focus:outline-none transition-all duration-300"
          />
        </div>
        <div>
          <label className="block text-sm font-medium text-gray-700">Địa chỉ</label>
          <input
            type="text"
            value={address}
            onChange={(e) => setAddress(e.target.value)}
            required
            className="mt-2 p-2 w-full border border-gray-300 rounded-lg shadow-sm focus:ring-2 focus:ring-blue-300 focus:outline-none transition-all duration-300"
          />
        </div>
        <button 
          type="submit" 
          className="w-full bg-gradient-to-r from-pink-500 to-red-500 text-white py-3 px-4 rounded-lg shadow-lg hover:bg-gradient-to-l focus:outline-none transition-all duration-300"
        >
          Đăng ký
        </button>
      </form>
      <div className="text-center mt-4">
        <p className="text-sm text-gray-600">
          Đã có tài khoản?{' '}
          <button 
            onClick={() => navigate('/login')} 
            className="text-pink-500 font-semibold hover:text-pink-700 focus:outline-none"
          >
            Đăng nhập
          </button>
        </p>
      </div>
    </div>
  );
};

export default RegisterForm;
