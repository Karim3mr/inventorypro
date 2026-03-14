import { useEffect, useState } from 'react'
import { productService, categoryService, supplierService } from '../services/api'
import Barcode from 'react-barcode'
import toast from 'react-hot-toast'
import { Plus, Search, Edit, Trash2, Barcode as BarcodeIcon, X } from 'lucide-react'
import { useAuth } from '../context/AuthContext'

const empty = {
  name: '', sku: '', barcode: '', description: '',
  price: '', costPrice: '', quantityInStock: 0,
  lowStockThreshold: 10, categoryId: '', supplierId: ''
}

export default function Products() {
  const { user } = useAuth()
  const canEdit = ['Admin', 'Manager'].includes(user?.role)
  const isAdmin = user?.role === 'Admin'

  const [products, setProducts] = useState([])
  const [categories, setCategories] = useState([])
  const [suppliers, setSuppliers] = useState([])
  const [search, setSearch] = useState('')
  const [filterCat, setFilterCat] = useState('')
  const [lowStock, setLowStock] = useState(false)
  const [showModal, setShowModal] = useState(false)
  const [editing, setEditing] = useState(null)
  const [form, setForm] = useState(empty)
  const [barcodeProduct, setBarcodeProduct] = useState(null)
  const [loading, setLoading] = useState(false)

  const load = () =>
    productService.getAll({ search, categoryId: filterCat || undefined, lowStock: lowStock || undefined })
      .then(r => setProducts(r.data))

  useEffect(() => {
    load()
    categoryService.getAll().then(r => setCategories(r.data))
    supplierService.getAll().then(r => setSuppliers(r.data))
  }, [search, filterCat, lowStock])

  const openCreate = () => { setEditing(null); setForm(empty); setShowModal(true) }
  const openEdit = (p) => {
    setEditing(p)
    setForm({
      name: p.name, sku: p.sku, barcode: p.barcode || '', description: p.description || '',
      price: p.price, costPrice: p.costPrice, quantityInStock: p.quantityInStock,
      lowStockThreshold: p.lowStockThreshold, categoryId: p.categoryId, supplierId: p.supplierId
    })
    setShowModal(true)
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    setLoading(true)
    try {
      const payload = { ...form, price: +form.price, costPrice: +form.costPrice, quantityInStock: +form.quantityInStock, lowStockThreshold: +form.lowStockThreshold, categoryId: +form.categoryId, supplierId: +form.supplierId }
      if (editing) {
        await productService.update(editing.id, { ...payload, isActive: true })
        toast.success('Product updated!')
      } else {
        await productService.create(payload)
        toast.success('Product created!')
      }
      setShowModal(false); load()
    } finally { setLoading(false) }
  }

  const handleDelete = async (id) => {
    if (!confirm('Deactivate this product?')) return
    await productService.delete(id)
    toast.success('Product deactivated.'); load()
  }

  return (
    <div className="space-y-5">
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-3">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Products</h1>
          <p className="text-gray-500 text-sm">{products.length} items</p>
        </div>
        {canEdit && <button onClick={openCreate} className="btn-primary flex items-center gap-2"><Plus className="w-4 h-4" />Add Product</button>}
      </div>

      {/* Filters */}
      <div className="flex flex-wrap gap-3">
        <div className="relative flex-1 min-w-[200px]">
          <Search className="absolute left-3 top-2.5 w-4 h-4 text-gray-400" />
          <input className="input pl-9" placeholder="Search name, SKU, barcode..."
            value={search} onChange={e => setSearch(e.target.value)} />
        </div>
        <select className="input w-auto" value={filterCat} onChange={e => setFilterCat(e.target.value)}>
          <option value="">All Categories</option>
          {categories.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
        </select>
        <label className="flex items-center gap-2 text-sm font-medium text-gray-700 cursor-pointer">
          <input type="checkbox" className="rounded" checked={lowStock} onChange={e => setLowStock(e.target.checked)} />
          Low Stock Only
        </label>
      </div>

      {/* Table */}
      <div className="card p-0 overflow-x-auto">
        <table className="w-full text-sm">
          <thead className="border-b border-gray-200">
            <tr className="text-left">
              {['Name', 'SKU', 'Category', 'Supplier', 'Stock', 'Price', 'Status', ''].map(h => (
                <th key={h} className="px-4 py-3 text-xs font-semibold text-gray-500 uppercase tracking-wide">{h}</th>
              ))}
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-100">
            {products.map(p => (
              <tr key={p.id} className="hover:bg-gray-50">
                <td className="px-4 py-3 font-medium text-gray-900">{p.name}</td>
                <td className="px-4 py-3 text-gray-500 font-mono text-xs">{p.sku}</td>
                <td className="px-4 py-3 text-gray-600">{p.categoryName}</td>
                <td className="px-4 py-3 text-gray-600">{p.supplierName}</td>
                <td className="px-4 py-3">
                  <span className={p.quantityInStock === 0 ? 'badge-red' : p.quantityInStock <= p.lowStockThreshold ? 'badge-yellow' : 'badge-green'}>
                    {p.quantityInStock} units
                  </span>
                </td>
                <td className="px-4 py-3 text-gray-900">${p.price.toFixed(2)}</td>
                <td className="px-4 py-3"><span className={p.isActive ? 'badge-green' : 'badge-red'}>{p.isActive ? 'Active' : 'Inactive'}</span></td>
                <td className="px-4 py-3">
                  <div className="flex items-center gap-2">
                    <button onClick={() => setBarcodeProduct(p)} className="p-1 text-gray-400 hover:text-blue-600" title="Barcode"><BarcodeIcon className="w-4 h-4" /></button>
                    {canEdit && <button onClick={() => openEdit(p)} className="p-1 text-gray-400 hover:text-blue-600"><Edit className="w-4 h-4" /></button>}
                    {isAdmin && <button onClick={() => handleDelete(p.id)} className="p-1 text-gray-400 hover:text-red-600"><Trash2 className="w-4 h-4" /></button>}
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        {products.length === 0 && <div className="py-12 text-center text-gray-400">No products found.</div>}
      </div>

      {/* Create/Edit Modal */}
      {showModal && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 p-4">
          <div className="bg-white rounded-xl shadow-xl w-full max-w-2xl max-h-[90vh] overflow-y-auto">
            <div className="flex items-center justify-between px-6 py-4 border-b">
              <h2 className="font-semibold text-gray-900">{editing ? 'Edit Product' : 'Add Product'}</h2>
              <button onClick={() => setShowModal(false)}><X className="w-5 h-5 text-gray-500" /></button>
            </div>
            <form onSubmit={handleSubmit} className="p-6 grid grid-cols-1 sm:grid-cols-2 gap-4">
              <div className="sm:col-span-2">
                <label className="label">Product Name *</label>
                <input className="input" required value={form.name} onChange={e => setForm({ ...form, name: e.target.value })} />
              </div>
              <div>
                <label className="label">SKU *</label>
                <input className="input" required value={form.sku} onChange={e => setForm({ ...form, sku: e.target.value })} disabled={!!editing} />
              </div>
              <div>
                <label className="label">Barcode</label>
                <input className="input" value={form.barcode} onChange={e => setForm({ ...form, barcode: e.target.value })} />
              </div>
              <div>
                <label className="label">Selling Price *</label>
                <input type="number" step="0.01" min="0" className="input" required value={form.price} onChange={e => setForm({ ...form, price: e.target.value })} />
              </div>
              <div>
                <label className="label">Cost Price *</label>
                <input type="number" step="0.01" min="0" className="input" required value={form.costPrice} onChange={e => setForm({ ...form, costPrice: e.target.value })} />
              </div>
              <div>
                <label className="label">Initial Stock</label>
                <input type="number" min="0" className="input" value={form.quantityInStock} onChange={e => setForm({ ...form, quantityInStock: e.target.value })} />
              </div>
              <div>
                <label className="label">Low Stock Alert Threshold</label>
                <input type="number" min="1" className="input" value={form.lowStockThreshold} onChange={e => setForm({ ...form, lowStockThreshold: e.target.value })} />
              </div>
              <div>
                <label className="label">Category *</label>
                <select className="input" required value={form.categoryId} onChange={e => setForm({ ...form, categoryId: e.target.value })}>
                  <option value="">Select category</option>
                  {categories.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
                </select>
              </div>
              <div>
                <label className="label">Supplier *</label>
                <select className="input" required value={form.supplierId} onChange={e => setForm({ ...form, supplierId: e.target.value })}>
                  <option value="">Select supplier</option>
                  {suppliers.map(s => <option key={s.id} value={s.id}>{s.name}</option>)}
                </select>
              </div>
              <div className="sm:col-span-2">
                <label className="label">Description</label>
                <textarea className="input" rows={2} value={form.description} onChange={e => setForm({ ...form, description: e.target.value })} />
              </div>
              <div className="sm:col-span-2 flex justify-end gap-3 pt-2">
                <button type="button" onClick={() => setShowModal(false)} className="btn-secondary">Cancel</button>
                <button type="submit" disabled={loading} className="btn-primary">{loading ? 'Saving...' : 'Save Product'}</button>
              </div>
            </form>
          </div>
        </div>
      )}

      {/* Barcode Modal */}
      {barcodeProduct && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 p-4">
          <div className="bg-white rounded-xl p-8 text-center space-y-4 shadow-xl">
            <h2 className="font-semibold text-gray-900">{barcodeProduct.name}</h2>
            <Barcode value={barcodeProduct.barcode || barcodeProduct.sku} />
            <p className="text-gray-500 text-sm">SKU: {barcodeProduct.sku}</p>
            <button onClick={() => setBarcodeProduct(null)} className="btn-secondary">Close</button>
          </div>
        </div>
      )}
    </div>
  )
}
