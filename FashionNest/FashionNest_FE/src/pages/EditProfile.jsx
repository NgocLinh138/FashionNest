import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Swal from 'sweetalert2';
import { jwtDecode } from 'jwt-decode';
import { getUserById, updateUserAPI } from '../services/userService';
import ProfileLayout from '../components/ProfileLayout'; 

const EditProfile = () => {
  const [user, setUser] = useState({
    userId: '',
    name: '',
    password: '',
    confirmPassword: '',
    phoneNumber: '',
    address: '',
  });

  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem('authToken');
    if (!token) {
      navigate('/login');
    } else {
      try {
        const decodedToken = jwtDecode(token);
        const userToken = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
        const userId = userToken.match(/Value\s*=\s*(.+?)\s*}/)?.[1];

        if (userId) {
          getUserById(userId)
            .then((data) => {
              setUser((prevUser) => ({
                ...prevUser,
                userId: userId,
                name: data.name,
                phoneNumber: data.phoneNumber,
                address: data.address,
              }));
            })
            .catch((error) => {
              console.error('Failed to fetch user:', error);
            });
        }
      } catch (error) {
        console.error('Error decoding token:', error);
      }
    }
  }, [navigate]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setUser((prevUser) => ({ ...prevUser, [name]: value }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    if (user.password !== user.confirmPassword) {
      Swal.fire({
        title: 'Lỗi!',
        text: 'Mật khẩu và xác nhận mật khẩu không khớp.',
        icon: 'error',
        confirmButtonText: 'OK',
      });
      return;
    }

    updateUserAPI(user)
      .then(() => {
        Swal.fire({
          title: 'Cập nhật thành công!',
          icon: 'success',
          showConfirmButton: false,
          showCancelButton: false, 
          background: '#fff',
          timer: 1000
        }).then(() => {
          navigate('/profile');
        });
      })
      .catch((error) => {
        console.error('Error updating user:', error);
        Swal.fire({
          title: 'Có lỗi xảy ra',
          text: 'Không thể cập nhật thông tin. Vui lòng thử lại sau.',
          icon: 'error',
          confirmButtonText: 'OK',
        });
      });
  };

  return (
    <ProfileLayout>
      <h2 className="text-4xl font-bold text-gray-800 text-center mb-8">Chỉnh sửa thông tin cá nhân</h2>
        <form onSubmit={handleSubmit}>
      
          <div className="mb-4">
            <label className="block text-gray-700">Tên</label>
            <input
              type="text"
              name="name"
              value={user.name}
              onChange={handleChange}
              className="w-full p-3 border border-gray-300 rounded-lg"
              required
            />
          </div>
          <div className="mb-4">
            <label className="block text-gray-700">Số điện thoại</label>
            <input
              type="text"
              name="phoneNumber"
              value={user.phoneNumber}
              onChange={handleChange}
              className="w-full p-3 border border-gray-300 rounded-lg"
              required
            />
          </div>
          <div className="mb-4">
            <label className="block text-gray-700">Địa chỉ</label>
            <input
              type="text"
              name="address"
              value={user.address}
              onChange={handleChange}
              className="w-full p-3 border border-gray-300 rounded-lg"
              required
            />
          </div>
          <div className="text-center">
            <button
              type="submit"
              className="bg-gradient-to-r from-pink-500 to-red-500 text-white text-lg py-3 px-8 rounded-lg hover:opacity-90 focus:outline-none focus:ring-4 focus:ring-pink-300 transition duration-300 shadow-md"
              >
              Lưu thay đổi
            </button>
          </div>
        </form>
    </ProfileLayout>
  );
};

export default EditProfile;
