import React, { useState, useEffect } from 'react';
import { getAllOrders, deleteOrder, updateOrderStatus } from '../../services/orderService';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSortUp, faSortDown, faArrowRight } from '@fortawesome/free-solid-svg-icons';
import Swal from 'sweetalert2';
import DataTable from '../../components/DataTable';

const ManageOrder = () => {
    const [orders, setOrders] = useState([]);
    const [loading, setLoading] = useState(true);
    const [searchTerm, setSearchTerm] = useState('');
    const [sortBy, setSortBy] = useState('price');
    const [sortOrderAscending, setSortOrderAscending] = useState(true);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [isUpdateModalOpen, setIsUpdateModalOpen] = useState(false);
    const [selectedOrder, setSelectedOrder] = useState(null);
    const [updateOrderData, setUpdateOrderData] = useState(null);

    useEffect(() => {
        const fetchOrders = async () => {
            try {
                const fetchedOrders = await getAllOrders(searchTerm, sortBy, sortOrderAscending);
                setOrders(fetchedOrders);
                setLoading(false);
            } catch (error) {
                console.error('Failed to fetch order data:', error);
                setLoading(false);
            }
        };

        fetchOrders();
    }, [searchTerm, sortBy, sortOrderAscending]);

    const handleSearchChange = (event) => {
        setSearchTerm(event.target.value);
    };

    const handleSortChange = (field) => {
        if (sortBy === field) {
            setSortOrderAscending(!sortOrderAscending);
        } else {
            setSortBy(field);
            setSortOrderAscending(true);
        }
    };

    const columns = [
        { field: 'id', label: 'ID' },
        { field: 'userId', label: 'User Id' },
        {
            field: 'status',
            label: 'Status',
            render: (row) => (
                <span
                    className={`inline-flex items-center px-3 py-1 rounded-full text-xs font-semibold 
                        ${row.status === 1 ? 'text-yellow-700 bg-yellow-100' :
                            row.status === 2 ? 'text-blue-700 bg-blue-100' :
                                row.status === 3 ? 'text-green-700 bg-green-100' :
                                    'text-red-700 bg-red-100'}`}
                >
                    {row.status === 1 ? 'Draft' :
                        row.status === 2 ? 'Pending' :
                            row.status === 3 ? 'Completed' : 'Cancelled'}
                </span>
            ),
        },
        { field: 'orderDate', label: 'Order Date' },
        { field: 'totalPrice', label: 'Total Price' },
        {
            field: 'view',
            label: 'View',
            render: (row) => (
                <span
                    onClick={() => openModal(row)}
                    className="cursor-pointer"
                >
                    <FontAwesomeIcon icon={faArrowRight} className="text-xl" />
                </span>
            ),
        },
    ];

    const statusOptions = [
        { value: 1, label: 'Draft' },
        { value: 2, label: 'Pending' },
        { value: 3, label: 'Completed' },
        { value: 4, label: 'Cancelled' },
    ];

    const openModal = (order) => {
        setSelectedOrder(order);
        setIsModalOpen(true);
    };

    const closeModal = () => {
        setIsModalOpen(false);
        setSelectedOrder(null);
    };

    const openUpdateModal = (order) => {
        setUpdateOrderData({ ...order });
        setIsUpdateModalOpen(true);
    };

    const closeUpdateModal = () => {
        setIsUpdateModalOpen(false);
        setUpdateOrderData(null);
    };

    const handleUpdate = async () => {
        try {
            const updatedOrderData = {
                orderId: updateOrderData.id,
                status: parseInt(updateOrderData.status, 10)
            };

            await updateOrderStatus(updatedOrderData.orderId, updatedOrderData.status);
            Swal.fire({
                title: 'Thành công!',
                text: 'Chi tiết đơn hàng được cập nhật thành công.',
                icon: 'success',
                showConfirmButton: false,
                showCancelButton: false,
                background: '#fff',
                timer: 1000
            })
            setIsUpdateModalOpen(false);

            const fetchedOrders = await getAllOrders(searchTerm, sortBy, sortOrderAscending);
            setOrders(fetchedOrders);
        } catch (error) {
            Swal.fire('Error!', 'There was an issue updating the order.', 'error');
        }
    };

    const handleDelete = async () => {
        const result = await Swal.fire({
            title: 'Are you sure?',
            text: "This action cannot be undone!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, delete it!',
            cancelButtonText: 'Cancel',
        });

        if (result.isConfirmed) {
            try {
                await deleteOrder(selectedOrder.id);
                setOrders(orders.filter((order) => order.id !== selectedOrder.id));
                closeModal();
                Swal.fire({
                    title: 'Thành công!',
                    text: 'Đơn hàng đã được xóa.',
                    icon: 'success',
                    showConfirmButton: false,
                    showCancelButton: false,
                    background: '#fff',
                    timer: 1000
                })
            } catch (error) {
                Swal.fire('Error!', 'There was an issue deleting the order.', 'error');
            }
        }
    };

    if (loading) {
        return <div className="text-center py-6">Loading data...</div>;
    }

    return (
        <div className="p-6 bg-white rounded-lg shadow-md">
            <h2 className="text-2xl font-semibold mb-6">Manage Orders</h2>

            <div className="mb-4">
                <input
                    type="text"
                    placeholder="Search orders..."
                    value={searchTerm}
                    onChange={handleSearchChange}
                    className="w-full p-2 border rounded-lg"
                />
            </div>

            <DataTable
                data={orders}
                columns={columns}
                sortBy={sortBy}
                sortOrderAscending={sortOrderAscending}
                onSortChange={handleSortChange}
                onRowClick={() => { }}
            />

            {isModalOpen && selectedOrder && (
                <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
                    <div className="bg-white p-6 rounded-lg w-96 shadow-lg transform transition-all scale-100 hover:scale-105">
                        <h3 className="text-xl font-semibold mb-6 text-center text-gray-800">Order Details</h3>

                        <div className="space-y-4">
                            <p><strong>ID:</strong> {selectedOrder.id}</p>
                            <p><strong>User ID:</strong> {selectedOrder.userId}</p>
                            <p><strong>Payment ID:</strong> {selectedOrder.paymentId}</p>
                            <p><strong>Status:</strong> {selectedOrder.status}</p>
                            <p><strong>Order Date:</strong> {selectedOrder.orderDate}</p>
                            <p><strong>Coupon ID:</strong> {selectedOrder.couponId}</p>
                            <p><strong>Total Price:</strong> {selectedOrder.totalPrice}</p>
                        </div>

                        <div className="mt-6 flex justify-between gap-4">
                            <button
                                onClick={() => openUpdateModal(selectedOrder)}
                                className="px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-300 transition"
                            >
                                Update
                            </button>
                            <button
                                onClick={handleDelete}
                                className="px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-red-300 transition"
                            >
                                Delete
                            </button>
                            <button
                                onClick={closeModal}
                                className="px-4 py-2 bg-gray-500 text-white rounded-lg hover:bg-gray-600 focus:outline-none focus:ring-2 focus:ring-gray-300 transition"
                            >
                                Close
                            </button>
                        </div>
                    </div>
                </div>
            )}

            {isUpdateModalOpen && updateOrderData && (
                <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
                    <div className="bg-white p-6 rounded-lg w-96 shadow-lg">
                        <h3 className="text-xl font-semibold mb-6 text-center text-gray-800">Update Order</h3>
                        <div className="space-y-4">
                            <div>
                                <label className="block text-gray-700">Status</label>
                                <select
                                    value={updateOrderData.status}
                                    onChange={(e) => setUpdateOrderData({ ...updateOrderData, status: e.target.value })}
                                    className="w-full p-2 border rounded"
                                >
                                    {statusOptions.map((status) => (
                                        <option key={status.value} value={status.value}>
                                            {status.label}
                                        </option>
                                    ))}
                                </select>
                            </div>
                        </div>
                        <div className="mt-6 flex justify-between gap-4">
                            <button
                                onClick={handleUpdate}
                                className="px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700"
                            >
                                Save
                            </button>
                            <button
                                onClick={closeUpdateModal}
                                className="px-4 py-2 bg-gray-500 text-white rounded-lg hover:bg-gray-600"
                            >
                                Cancel
                            </button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}

export default ManageOrder;
