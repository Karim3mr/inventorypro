import axios from 'axios'
import toast from 'react-hot-toast'

const api = axios.create({ baseURL: '/api' })

api.interceptors.response.use(
  res => res,
  err => {
    const msg = err.response?.data?.message || 'Something went wrong'
    if (err.response?.status === 401) {
      localStorage.removeItem('token')
      localStorage.removeItem('user')
      window.location.href = '/login'
    } else {
      toast.error(msg)
    }
    return Promise.reject(err)
  }
)

export default api

// ── Auth ──────────────────────────────────────────────────
export const authService = {
  login: (data) => api.post('/auth/login', data),
  register: (data) => api.post('/auth/register', data),
}

// ── Products ──────────────────────────────────────────────
export const productService = {
  getAll: (params) => api.get('/products', { params }),
  get: (id) => api.get(`/products/${id}`),
  create: (data) => api.post('/products', data),
  update: (id, data) => api.put(`/products/${id}`, data),
  delete: (id) => api.delete(`/products/${id}`),
  getBarcode: (id) => api.get(`/products/${id}/barcode`),
}

// ── Categories ────────────────────────────────────────────
export const categoryService = {
  getAll: () => api.get('/categories'),
  create: (data) => api.post('/categories', data),
  update: (id, data) => api.put(`/categories/${id}`, data),
  delete: (id) => api.delete(`/categories/${id}`),
}

// ── Suppliers ─────────────────────────────────────────────
export const supplierService = {
  getAll: () => api.get('/suppliers'),
  get: (id) => api.get(`/suppliers/${id}`),
  create: (data) => api.post('/suppliers', data),
  update: (id, data) => api.put(`/suppliers/${id}`, data),
  delete: (id) => api.delete(`/suppliers/${id}`),
}

// ── Stock ─────────────────────────────────────────────────
export const stockService = {
  getTransactions: (params) => api.get('/stock', { params }),
  createTransaction: (data) => api.post('/stock', data),
}

// ── Alerts ────────────────────────────────────────────────
export const alertService = {
  getAll: (params) => api.get('/alerts', { params }),
  resolve: (id) => api.patch(`/alerts/${id}/resolve`),
}

// ── Reports ───────────────────────────────────────────────
export const reportService = {
  dashboard: () => api.get('/reports/dashboard'),
  stockMovement: (params) => api.get('/reports/stock-movement', { params }),
  topProducts: () => api.get('/reports/top-products'),
  inventoryValue: () => api.get('/reports/inventory-value'),
}
