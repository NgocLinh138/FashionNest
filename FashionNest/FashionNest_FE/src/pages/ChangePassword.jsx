import React, { useState, useEffect} from 'react';
import Swal from 'sweetalert2';
import { useNavigate } from 'react-router-dom';
import { changePassword } from '../services/authService';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import ProfileLayout from '../components/ProfileLayout'; 
import { jwtDecode } from 'jwt-decode';
import { getUserById } from '../services/userService'; 

const ChangePassword = () => {
  const [currentPassword, setCurrentPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [email, setEmail] = useState(''); 
  const navigate = useNavigate();
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);

  const token = localStorage.getItem('authToken');
  const decodedToken = jwtDecode(token);
  const userToken = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
  const userId = userToken.match(/Value\s*=\s*(.+?)\s*}/)?.[1];

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const userData = await getUserById(userId);
        setEmail(userData.email); 
      } catch (error) {
        console.error('Error fetching user:', error);
        Swal.fire({
          icon: 'error',
          title: 'Lỗi',
          text: 'Không thể lấy thông tin người dùng.',
        });
      }
    };

    fetchUser();
  }, [userId]);

  const handleSubmit = async (e) => {
    e.preventDefault();
  
    if (newPassword !== confirmPassword) {
      Swal.fire({
        icon: 'error',
        title: 'Lỗi',
        text: 'Mật khẩu xác nhận không khớp!',
      });
      return;
    }

    const result = await Swal.fire({
        title: 'Bạn có chắc chắn muốn thay đổi mật khẩu?',
        text: 'Hành động này không thể hoàn tác.',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Đồng ý',
        cancelButtonText: 'Hủy',
      });
  
    if (result.isConfirmed) {
      try {
        await changePassword(email, currentPassword, newPassword, confirmPassword);
        
        Swal.fire({
          icon: 'success',
          title: 'Thành công!',
          text: 'Mật khẩu đã được thay đổi thành công!',
          showConfirmButton: false,
          showCancelButton: false, 
          background: '#fff',
          timer: 1000
        }).then(() => {
          navigate('/profile'); 
        });
      } catch (err) {
        console.error('Error:', err.response ? err.response.data : err.message);
        
        Swal.fire({
          icon: 'error',
          title: 'Có lỗi xảy ra',
          text: err.response ? err.response.data.message || 'Có lỗi xảy ra khi thay đổi mật khẩu.' : 'Có lỗi xảy ra.',
        });
      }
    }
  };

  const togglePasswordVisibility = () => {
    setShowPassword(!showPassword);
  };

  const toggleConfirmPasswordVisibility = () => {
    setShowConfirmPassword(!showConfirmPassword);
  };


  return (
    <ProfileLayout>
      <h2 className="text-4xl font-bold text-gray-800 text-center mb-8">Đổi mật khẩu</h2>
      <form onSubmit={handleSubmit}>
        <div className="mb-6">
          <div className="mb-6">
            <label className="block text-lg font-semibold text-gray-700">Email</label>
            <input
              type="email"
              value={email}
              disabled
              className="w-full px-4 py-3 mt-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-indigo-500"
            />
          </div>

          <label className="block text-lg font-semibold text-gray-700">Mật khẩu hiện tại</label>
          <div className="relative">
            <input
              type={showPassword ? 'text' : 'password'}
              value={currentPassword}
              onChange={(e) => setCurrentPassword(e.target.value)}
              required
              className="w-full px-4 py-3 mt-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-indigo-500"
            />
            <FontAwesomeIcon
              icon={showPassword ? faEyeSlash : faEye}
              className="absolute top-1/2 right-3 transform -translate-y-1/2 cursor-pointer"
              onClick={togglePasswordVisibility}
            />
          </div>
        </div>

        <div className="mb-6">
          <label className="block text-lg font-semibold text-gray-700">Mật khẩu mới</label>
          <div className="relative">
            <input
              type={showPassword ? 'text' : 'password'}
              value={newPassword}
              onChange={(e) => setNewPassword(e.target.value)}
              required
              className="w-full px-4 py-3 mt-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-indigo-500"
            />
            <FontAwesomeIcon
              icon={showPassword ? faEyeSlash : faEye}
              className="absolute top-1/2 right-3 transform -translate-y-1/2 cursor-pointer"
              onClick={togglePasswordVisibility}
            />
          </div>
        </div>

        <div className="mb-6">
          <label className="block text-lg font-semibold text-gray-700">Xác nhận mật khẩu mới</label>
          <div className="relative">
            <input
              type={showConfirmPassword ? 'text' : 'password'}
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              required
              className="w-full px-4 py-3 mt-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-indigo-500"
            />
            <FontAwesomeIcon
              icon={showConfirmPassword ? faEyeSlash : faEye}
              className="absolute top-1/2 right-3 transform -translate-y-1/2 cursor-pointer"
              onClick={toggleConfirmPasswordVisibility}
            />
          </div>
        </div>

        <div className="text-center">
          <button
            type="submit"
            className="bg-gradient-to-r from-pink-500 to-red-500 text-white text-lg py-3 px-8 rounded-lg hover:opacity-90 focus:outline-none focus:ring-4 focus:ring-pink-300 transition duration-300 shadow-md"
            >
            Đổi mật khẩu
          </button>
        </div>
      </form>

    </ProfileLayout>
  );
};

export default ChangePassword;
