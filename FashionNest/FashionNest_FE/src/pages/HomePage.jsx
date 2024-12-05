import React, { useEffect, useState } from 'react';
import { getAllProducts } from '../services/productService';
import { useNavigate } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSearch } from '@fortawesome/free-solid-svg-icons';

const HomePage = () => {
  const [products, setProducts] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const data = await getAllProducts();
        setProducts(data);
      } catch (err) {
        setError('Không thể lấy danh sách sản phẩm. Vui lòng thử lại sau.');
      }
    };

    fetchProducts();
  }, []);

  const handleSearch = async (event) => {
    const term = event.target.value;
    setSearchTerm(term);

    if (term.trim() === '') {
      const data = await getAllProducts();
      setProducts(data);
    } else {
      try {
        const data = await getAllProducts(term);
        setProducts(data);
      } catch (err) {
        setError('Không thể tìm kiếm sản phẩm. Vui lòng thử lại sau.');
      }
    }
  };

  const handleSearchIconClick = async () => {
    if (searchTerm.trim() !== '') {
      try {
        const data = await getAllProducts(searchTerm);
        setProducts(data);
      } catch (err) {
        setError('Không thể tìm kiếm sản phẩm. Vui lòng thử lại sau.');
      }
    }
  };

  const goToProductDetail = (id) => {
    navigate(`/product/${id}`);
  };

  return (
    <div className="bg-gray-50 min-h-screen pt-16">
      <section className="relative w-full h-72 sm:h-96 bg-cover bg-center shadow-lg"
        style={{ backgroundImage: 'url(https://res.cloudinary.com/dyhv46k4r/image/upload/v1732429882/10766663_gjczgm.jpg)' }}>
        <div className="absolute inset-0 bg-black opacity-40 rounded-xl"></div>
        <div className="absolute inset-0 flex justify-center items-center text-white text-4xl sm:text-5xl font-bold px-4 text-center">
        </div>
      </section>

      <main className="container mx-auto px-4 py-6">
        <div className="mb-6 flex justify-center">
          <div className="relative w-full sm:w-2/3 lg:w-1/2">
            <input
              type="text"
              placeholder="Tìm kiếm sản phẩm..."
              value={searchTerm}
              onChange={handleSearch}
              className="w-full p-2 border-2 border-gray-300 rounded-full shadow-lg focus:outline-none focus:ring-2 focus:ring-indigo-500 pl-12 text-lg"
            />
            <FontAwesomeIcon
              icon={faSearch}
              className="absolute left-4 top-1/2 transform -translate-y-1/2 text-gray-500 cursor-pointer"
              onClick={handleSearchIconClick}
            />
          </div>
        </div>

        {error && <p className="text-red-500 text-center">{error}</p>}

        {/* Product Grid */}
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-8">
          {products.length > 0 ? (
            products.map((product) => (
              <div
                key={product.id}
                className="bg-white p-6 rounded-3xl shadow-xl cursor-pointer transition-all duration-300 ease-in-out transform hover:scale-105 hover:shadow-2xl hover:bg-gray-100"
                onClick={() => goToProductDetail(product.id)}
              >
                <img
                  src={product.image}
                  alt={product.name}
                  className="w-full h-56 object-cover rounded-2xl mb-4 transition-all duration-300 ease-in-out transform hover:scale-105"
                />
                <h3 className="text-lg font-semibold text-gray-800 mb-2">{product.name}</h3>
                <p className="text-sm text-gray-600 mb-2">{product.description}</p>
                <p className="text-xl text-pink-500 font-bold">{product.price.toLocaleString()} VND</p>
                <p className="text-sm text-gray-500">Còn lại: {product.stock}</p>
              </div>
            ))
          ) : (
            <p className="text-center text-gray-500">Không có sản phẩm nào phù hợp với từ khóa tìm kiếm.</p>
          )}
        </div>
      </main>
    </div>
  );
};

export default HomePage;
