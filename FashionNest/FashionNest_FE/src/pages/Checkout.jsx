import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import Swal from 'sweetalert2';
import { jwtDecode } from 'jwt-decode';
import { getUserById } from '../services/userService';
import { createOrder } from '../services/orderService';
import { updatePaymentStatus } from '../services/paymentService';

const Checkout = () => {
  const [cart, setCart] = useState([]);
  const [user, setUser] = useState({});
  const [paymentMethod, setPaymentMethod] = useState('COD');
  const navigate = useNavigate();

  useEffect(() => {
    const savedCart = localStorage.getItem('cart');
    setCart(savedCart ? JSON.parse(savedCart) : []);

    const token = localStorage.getItem('authToken');
    if (token) {
      try {
        const decodedToken = jwtDecode(token);
        const userToken = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
        const userId = userToken.match(/Value\s*=\s*(.+?)\s*}/)?.[1];

        if (userId) {
          getUserById(userId).then(userData => {
            setUser(userData);
          }).catch((error) => {
            console.error('Failed to fetch user:', error);
          });
        }
      } catch (error) {
        console.error('Error decoding token:', error);
      }
    }
  }, []);

  const calculateTotal = () => {
    return cart.reduce((total, item) => total + (item.price * item.quantity), 0);
  };

  const total = calculateTotal();

  const handlePaymentMethodChange = (method) => {
    setPaymentMethod(method);
  };

  const handlePlaceOrder = () => {
    if (!user.name || !user.phoneNumber || !user.address) {
      Swal.fire({
        title: 'Thiếu thông tin',
        text: 'Vui lòng cung cấp đầy đủ thông tin người nhận.',
        icon: 'warning',
        confirmButtonText: 'Ok',
      });
      return;
    }

    const orderData = {
      userId: user.id,
      couponId: null,
      orderItems: cart.map(item => ({
        productId: item.id,
        quantity: item.quantity,
        price: item.price,
      })),
      paymentMethod: paymentMethod === 'COD' ? 1 : (paymentMethod === 'VNPAY' ? 2 : 1),
    };

    console.log('Order Data:', orderData);

    Swal.fire({
      title: 'Xác nhận đặt hàng?',
      text: `Bạn có chắc chắn muốn đặt hàng với tổng giá trị ${total.toLocaleString()} VND?`,
      icon: 'question',
      showCancelButton: true,
      confirmButtonText: 'Đặt hàng',
      cancelButtonText: 'Hủy',
    }).then((result) => {
      if (result.isConfirmed) {
        createOrder(orderData)
          .then(order => {
            console.log(order);
            if (order.paymentMethod === 2) {
              window.location.href = order.paymentUrl;
            } else {
              Swal.fire({
                title: 'Đặt hàng thành công!',
                text: 'Chúng tôi sẽ xác nhận đơn hàng của bạn trong thời gian sớm nhất.',
                icon: 'success',
                showConfirmButton: false,
                showCancelButton: false,
                background: '#fff',
                timer: 2000
              }).then(() => {
                localStorage.removeItem('cart');
                navigate('/');
              });
            };
          })
          .catch((error) => {
            Swal.fire({
              title: 'Có lỗi xảy ra!',
              text: 'Không thể tạo đơn hàng. Vui lòng thử lại sau.',
              icon: 'error',
              confirmButtonText: 'OK',
            });
          });
      }
    });
  };

  return (
    <div className="bg-gray-50 min-h-screen py-12 px-4 sm:px-6 lg:px-8 mt-16">
      <div className="max-w-5xl mx-auto bg-white p-8 rounded-xl shadow-lg">
        <h2 className="text-4xl font-bold text-gray-800 text-center mb-8">Thông tin thanh toán</h2>

        <div className="mb-8">
          <h3 className="text-xl font-semibold">Thông tin người đặt hàng</h3>
          <div className="mt-4">
            <p><strong>Tên: </strong>{user.name || 'Chưa có tên'}</p>
            <p><strong>Số điện thoại: </strong>{user.phoneNumber || 'Chưa có số điện thoại'}</p>
            <p><strong>Địa chỉ: </strong>{user.address || 'Chưa có địa chỉ'}</p>
          </div>
        </div>

        <div className="mb-8">
          <h3 className="text-xl font-semibold">Sản phẩm trong giỏ hàng</h3>
          {cart.length === 0 ? (
            <p>Giỏ hàng trống</p>
          ) : (
            <div>
              {cart.map((item) => (
                <div key={item.id} className="flex justify-between py-4 border-b border-gray-300">
                  <div>
                    <h4 className="text-lg font-semibold">{item.name}</h4>
                    <p>{item.quantity} x {item.price.toLocaleString()} VND</p>
                  </div>
                  <div>
                    <p>{(item.price * item.quantity).toLocaleString()} VND</p>
                  </div>
                </div>
              ))}
              <div className="mt-4 flex justify-between">
                <h4 className="text-lg font-semibold">Tổng tiền:</h4>
                <p className="text-lg font-semibold text-pink-500">{total.toLocaleString()} VND</p>
              </div>
            </div>
          )}
        </div>

        <div className="mb-8">
          <h3 className="text-xl font-semibold">Chọn phương thức thanh toán</h3>
          <div className="mt-4">
            <label>
              <input
                type="radio"
                name="paymentMethod"
                value="COD"
                checked={paymentMethod === 'COD'}
                onChange={() => handlePaymentMethodChange('COD')}
                className="mr-2"
              />
              Thanh toán khi nhận hàng (COD)
            </label>
            <br />
            <label>
              <input
                type="radio"
                name="paymentMethod"
                value="VNPAY"
                checked={paymentMethod === 'VNPAY'}
                onChange={() => handlePaymentMethodChange('VNPAY')}
                className="mr-2"
              />
              Thanh toán qua VNPAY
            </label>
          </div>
        </div>

        <div className="mt-6 text-center">
          <button
            onClick={handlePlaceOrder}
            className="bg-gradient-to-r from-pink-500 to-red-500 text-white py-3 px-12 rounded-lg hover:bg-green-600 focus:outline-none transition duration-300"
          >
            Đặt hàng
          </button>
        </div>
      </div>
    </div>
  );
};

export default Checkout;
