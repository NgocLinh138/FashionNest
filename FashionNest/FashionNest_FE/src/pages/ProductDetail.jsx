import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getProductById } from '../services/productService';
import Swal from 'sweetalert2';

const ProductDetail = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [product, setProduct] = useState(null);
  const [error, setError] = useState('');

  useEffect(() => {
    const fetchProduct = async () => {
      try {
        const data = await getProductById(id);
        setProduct(data.product);
      } catch (err) {
        setError('Không thể lấy thông tin sản phẩm. Vui lòng thử lại sau.');
      }
    };

    fetchProduct();
  }, [id]);

  const addToCart = () => {
    const cartItem = {
      id: product.id,
      name: product.name,
      price: product.price,
      image: product.image,
      quantity: 1,
    };

    let cart = JSON.parse(localStorage.getItem('cart')) || [];
    const existingItemIndex = cart.findIndex(item => item.id === product.id);

    if (existingItemIndex !== -1) {
      cart[existingItemIndex].quantity += 1;
    } else {
      cart.push(cartItem);
    }

    localStorage.setItem('cart', JSON.stringify(cart));

    window.dispatchEvent(new Event('storage'));

    Swal.fire({
      title: 'Thêm vào giỏ hàng!',
      text: 'Sản phẩm đã được thêm vào giỏ hàng.',
      icon: 'success',
      showConfirmButton: false,
      showCancelButton: false,
      background: '#fff',
      timer: 1000
    });
  };


  if (error) {
    return <p className="text-center text-red-500">{error}</p>;
  }

  if (!product) {
    return <p className="text-center">Đang tải thông tin sản phẩm...</p>;
  }

  return (
    <div className="bg-gray-100 min-h-screen py-12 px-4 sm:px-6 lg:px-8 mt-16">
      <div className="max-w-4xl mx-auto bg-white p-6 rounded-lg shadow-lg">
        <div className="flex flex-col md:flex-row justify-between items-center mt-6">
          <img
            src={product.image}
            alt={product.name}
            className="w-full md:w-1/2 h-auto object-cover rounded-lg shadow-lg mb-6 md:mb-0"
          />
          <div className="md:w-1/2 md:pl-8 text-center md:text-left">
            <h2 className="text-3xl font-semibold text-gray-800">{product.name}</h2>
            <p className="text-lg text-gray-700 mt-5">{product.description}</p>
            <p className="text-xl font-semibold text-pink-500 mt-3">
              Giá: {product.price.toLocaleString()} VND
            </p>
            <p className="text-gray-400 mt-3">Còn lại: {product.stock}</p>
            <p className="text-gray-600 mt-3">Danh mục: {product.category.name}</p>
            <div className="mt-6 flex justify-center md:justify-start gap-4">
              <button
                onClick={addToCart}
                className="bg-white text-pink-500 border-2 border-pink-500 py-2 px-6 rounded-lg hover:bg-gradient-to-r from-pink-500 to-red-500 hover:text-white hover:border-2 focus:outline-none"
              >
                Thêm vào giỏ hàng
              </button>
              <button
                onClick={() => navigate('/cart')}
                className="bg-gradient-to-r from-pink-500 to-red-500 text-white py-2 px-6 rounded-lg hover:bg-green-600 focus:outline-none"
              >
                Đi đến giỏ hàng
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ProductDetail;
