import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom'; 
import Swal from 'sweetalert2'; 
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTrash } from '@fortawesome/free-solid-svg-icons'; 

const Cart = () => {
  const [cart, setCart] = useState([]);
  const navigate = useNavigate();  

  useEffect(() => {
    const savedCart = JSON.parse(localStorage.getItem('cart')) || [];
    setCart(savedCart);
  }, []);

  const handleRemoveItem = (id) => {
    Swal.fire({
      title: 'Bạn có chắc chắn muốn xóa sản phẩm này?',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Xóa',
      cancelButtonText: 'Hủy',
    }).then((result) => {
      if (result.isConfirmed) {
        const updatedCart = cart.filter(item => item.id !== id);
        setCart(updatedCart);
        localStorage.setItem('cart', JSON.stringify(updatedCart)); 
        window.dispatchEvent(new Event('storage'));

        Swal.fire({
          title: 'Đã xóa!',
          text: 'Sản phẩm đã được xóa khỏi giỏ hàng.',
          icon: 'success',
          showConfirmButton: false,
          showCancelButton: false, 
          background: '#fff',
          timer: 1000,  
        });
      }
    });
  };

  const handleCheckout = () => {
    const isLoggedIn = JSON.parse(localStorage.getItem('isLoggedIn')) || false;

    if (!isLoggedIn) {
      Swal.fire({
        title: 'Cần đăng nhập',
        text: 'Vui lòng đăng nhập để tiếp tục thanh toán.',
        icon: 'warning',
        confirmButtonText: 'Đăng nhập',
      }).then(() => {
        navigate('/login'); 
      });
    } else {
      localStorage.setItem('cart', JSON.stringify(cart));
      navigate('/checkout'); 
    }
  };

  const calculateTotal = () => {
    return cart.reduce((total, item) => total + (item.price * item.quantity), 0);
  };

  const total = calculateTotal();

  const handleProductClick = (id) => {
    navigate(`/product/${id}`);
  };

  return (
    <div className="bg-gray-50 min-h-screen py-12 px-4 sm:px-6 lg:px-8 mt-16">
      <div className="max-w-5xl mx-auto bg-white p-8 rounded-xl shadow-lg">
        <h2 className="text-4xl font-bold text-gray-800 text-center mb-8">Giỏ hàng của bạn</h2>
        {cart.length === 0 ? (
          <div className="text-center">
            <img 
              src="https://cdn-icons-png.flaticon.com/512/11329/11329060.png" 
              alt="Empty cart" 
              className="mx-auto mb-6 w-40 h-40"
            />
            <p className="text-center text-gray-600">Giỏ hàng của bạn đang trống.</p>
            <div className="mt-6 flex justify-center gap-4">
              <button 
                onClick={() => navigate('/')}
                className="bg-gradient-to-r from-pink-500 to-red-500 text-white py-3 px-12 rounded-lg hover:bg-green-600 focus:outline-none transition duration-300"
              >
                Mua ngay
              </button>
            </div>
          </div>
          ) : (
          <div>
            {cart.map((item) => (
              <div 
                key={item.id} 
                className="flex items-center justify-between py-6 border-b border-gray-300 cursor-pointer hover:bg-gray-50 transition duration-300"
                onClick={() => handleProductClick(item.id)}
              >
                <img src={item.image} alt={item.name} className="w-20 h-20 object-cover rounded-lg shadow-md" />
                <div className="ml-6 flex-1">
                  <h4 className="text-xl font-semibold text-gray-700">{item.name}</h4>
                  <p className="text-gray-600 text-sm">{item.price.toLocaleString()} VND</p>
                  <p className="text-gray-600 text-sm">Số lượng: {item.quantity}</p>
                </div>
                <button 
                  onClick={(e) => { 
                    e.stopPropagation(); 
                    handleRemoveItem(item.id); 
                  }} 
                  className="transition duration-300"
                >
                  <FontAwesomeIcon icon={faTrash} size="lg" style={{ color: '#B91C1C' }}/>
                </button>
              </div>
            ))}
            <div className="mt-6 flex justify-between items-center">
              <h3 className="text-lg font-semibold text-gray-800">Tổng tiền:</h3>
              <p className="text-lg font-semibold text-pink-500">{total.toLocaleString()} VND</p>
            </div>
            <div className="mt-6 text-center">
              <button
                onClick={handleCheckout}
                className="bg-gradient-to-r from-pink-500 to-red-500 text-white py-3 px-12 rounded-lg hover:bg-green-600 focus:outline-none transition duration-300"
              >
                Thanh toán
              </button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default Cart;
