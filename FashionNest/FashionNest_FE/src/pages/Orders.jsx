import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";
import { getOrdersByUserId } from "../services/orderService";
import ProfileLayout from "../components/ProfileLayout";

const Orders = () => {
  const [orders, setOrders] = useState([]);
  const [filteredOrders, setFilteredOrders] = useState([]);
  const [activeStatus, setActiveStatus] = useState('all');
  const navigate = useNavigate();

  useEffect(() => {
    const token = localStorage.getItem("authToken");
    if (!token) {
      navigate("/login");
    } else {
      try {
        const decodedToken = jwtDecode(token);
        const userToken =
          decodedToken[
            "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
          ];
        const userId = userToken.match(/Value\s*=\s*(.+?)\s*}/)?.[1];

        if (userId) {
          getOrdersByUserId(userId)
            .then((orderData) => {
              setOrders(orderData);
              setFilteredOrders(orderData); 
            })
            .catch((error) => {
              console.error("Error fetching orders:", error);
            });
        }
      } catch (error) {
        console.error("Error decoding token:", error);
      }
    }
  }, [navigate]);

  const handleFilterChange = (status) => {
    setActiveStatus(status);
    if (status === 'all') {
      setFilteredOrders(orders); 
    } else {
      const filtered = orders.filter(order => order.status === status);
      setFilteredOrders(filtered);
    }
  };

  return (
    <ProfileLayout>
      <h2 className="text-4xl font-bold text-gray-800 text-center mb-8">
        Danh sách đơn hàng
      </h2>

      <div className="flex justify-center space-x-4 mb-6">
        <button
          onClick={() => handleFilterChange('all')}
          className={`px-4 py-2 rounded ${activeStatus === 'all' ? 'bg-gradient-to-r from-pink-500 to-red-500 text-white' : 'bg-gray-200'}`}
        >
          Tất cả
        </button>
        <button
          onClick={() => handleFilterChange(1)} // Status 1 - Draft
          className={`px-4 py-2 rounded ${activeStatus === 1 ? 'bg-gradient-to-r from-pink-500 to-red-500 text-white' : 'bg-gray-200'}`}
        >
          Draft
        </button>
        <button
          onClick={() => handleFilterChange(2)} // Status 2 - Pending
          className={`px-4 py-2 rounded ${activeStatus === 2 ? 'bg-gradient-to-r from-pink-500 to-red-500 text-white' : 'bg-gray-200'}`}
        >
          Pending
        </button>
        <button
          onClick={() => handleFilterChange(3)} // Status 3 - Completed
          className={`px-4 py-2 rounded ${activeStatus === 3 ? 'bg-gradient-to-r from-pink-500 to-red-500 text-white' : 'bg-gray-200'}`}
        >
          Completed
        </button>
        <button
          onClick={() => handleFilterChange(4)} // Status 4 - Cancelled
          className={`px-4 py-2 rounded ${activeStatus === 4 ? 'bg-gradient-to-r from-pink-500 to-red-500 text-white' : 'bg-gray-200'}`}
        >
          Cancelled
        </button>
      </div>

      <div>
        {filteredOrders.length > 0 ? (
          <ul>
            {filteredOrders.map((order) => (
              <li key={order.id} className="mb-4">
                <div className="p-4 border border-gray-300 rounded-lg">
                  <p>
                    <strong>Đơn hàng ID:</strong> {order.id}
                  </p>
                  <p>
                    <strong>Ngày đặt:</strong> {order.orderDate}
                  </p>
                  <p>
                    <strong>Tổng tiền:</strong> {order.totalPrice}
                  </p>
                  <p>
                    <strong>Trạng thái:</strong>{" "}
                    {order.status === 1
                      ? "Draft"
                      : order.status === 2
                      ? "Pending"
                      : order.status === 3
                      ? "Completed"
                      : "Cancelled"}
                  </p>
                </div>
              </li>
            ))}
          </ul>
        ) : (
          <p>Chưa có đơn hàng nào.</p>
        )}
      </div>
    </ProfileLayout>
  );
};

export default Orders;
