import { useEffect, useState } from 'react'
import { stockService, productService } from '../services/api'
import toast from 'react-hot-toast'
import { Plus, ArrowDownCircle, ArrowUpCircle, RefreshCw, X } from 'lucide-react'

const TypeIcon = ({ type }) => {
  if (type === 'IN') return <ArrowDownCircle className="w-4 h-4 text-green-500" />
  if (type === 'OUT') return <ArrowUpCircle className="w-4 h-4 text-red-500" />
  return <RefreshCw className="w-4 h-4 text-blue-500" />
}

export default function Stock() {
  const [transactions, setTransactions] = useState([])
  const [products, setProducts] = useState([])
  const [showModal, setShowModal] = useState(false)
  const [form, setForm] = useState({ productId: '', transactionType: 'IN', quantity: 1, notes: '' })
  const [loading, setLoading] = useState(false)

  const load = () => stockService.getTransactions().then(r => setTransactions(r.data))
  useEffect(() => {
    load()
    productService.getAll().then(r => setProducts(r.data))
  }, [])

  const handleSubmit = async (e) => {
    e.preventDefault()
    setLoading(true)
    try {
      const res = await stockService.createTransaction({ ...form, productId: +form.productId, quantity: +form.quantity })
      toast.success(`Stock updated! New qty: ${res.data.newQuantity}`)
      setShowModal(false); load()
    } finally { setLoading(false) }
  }

  return (
    <div className="space-y-5">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Stock Transactions</h1>
          <p className="text-gray-500 text-sm">Last 200 transactions</p>
        </div>
        <button onClick={() => setShowModal(true)} className="btn-primary flex items-center gap-2">
          <Plus className="w-4 h-4" />New Transaction
        </button>
      </div>

      <div className="card p-0 overflow-x-auto">
        <table className="w-full text-sm">
          <thead className="border-b border-gray-200">
            <tr>
              {['Type', 'Product', 'SKU', 'Qty', 'Before', 'After', 'Notes', 'By', 'Date'].map(h => (
                <th key={h} className="px-4 py-3 text-left text-xs font-semibold text-gray-500 uppercase">{h}</th>
              ))}
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-100">
            {transactions.map(t => (
              <tr key={t.id} className="hover:bg-gray-50">
                <td className="px-4 py-3"><div className="flex items-center gap-2"><TypeIcon type={t.transactionType} /><span className={t.transactionType === 'IN' ? 'text-green-700' : t.transactionType === 'OUT' ? 'text-red-700' : 'text-blue-700'}>{t.transactionType}</span></div></td>
                <td className="px-4 py-3 font-medium text-gray-900">{t.productName}</td>
                <td className="px-4 py-3 font-mono text-xs text-gray-500">{t.sku}</td>
                <td className="px-4 py-3 font-semibold">{t.quantity}</td>
                <td className="px-4 py-3 text-gray-500">{t.quantityBefore}</td>
                <td className="px-4 py-3 font-medium">{t.quantityAfter}</td>
                <td className="px-4 py-3 text-gray-500 max-w-[150px] truncate">{t.notes || '—'}</td>
                <td className="px-4 py-3 text-gray-500">{t.userName}</td>
                <td className="px-4 py-3 text-gray-500 whitespace-nowrap">{new Date(t.createdAt).toLocaleDateString()}</td>
              </tr>
            ))}
          </tbody>
        </table>
        {transactions.length === 0 && <div className="py-12 text-center text-gray-400">No transactions yet.</div>}
      </div>

      {showModal && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 p-4">
          <div className="bg-white rounded-xl shadow-xl w-full max-w-md">
            <div className="flex items-center justify-between px-6 py-4 border-b">
              <h2 className="font-semibold">New Stock Transaction</h2>
              <button onClick={() => setShowModal(false)}><X className="w-5 h-5 text-gray-500" /></button>
            </div>
            <form onSubmit={handleSubmit} className="p-6 space-y-4">
              <div>
                <label className="label">Product *</label>
                <select className="input" required value={form.productId} onChange={e => setForm({ ...form, productId: e.target.value })}>
                  <option value="">Select product</option>
                  {products.map(p => <option key={p.id} value={p.id}>{p.name} (Stock: {p.quantityInStock})</option>)}
                </select>
              </div>
              <div>
                <label className="label">Transaction Type *</label>
                <div className="grid grid-cols-3 gap-2">
                  {['IN', 'OUT', 'ADJUSTMENT'].map(t => (
                    <button key={t} type="button"
                      className={`py-2 rounded-lg border text-sm font-medium transition-colors ${form.transactionType === t ? 'border-blue-600 bg-blue-50 text-blue-700' : 'border-gray-300 text-gray-600 hover:border-blue-400'}`}
                      onClick={() => setForm({ ...form, transactionType: t })}>
                      {t}
                    </button>
                  ))}
                </div>
              </div>
              <div>
                <label className="label">{form.transactionType === 'ADJUSTMENT' ? 'New Quantity' : 'Quantity'} *</label>
                <input type="number" min="1" className="input" required value={form.quantity} onChange={e => setForm({ ...form, quantity: e.target.value })} />
              </div>
              <div>
                <label className="label">Notes</label>
                <textarea className="input" rows={2} value={form.notes} onChange={e => setForm({ ...form, notes: e.target.value })} />
              </div>
              <div className="flex justify-end gap-3">
                <button type="button" onClick={() => setShowModal(false)} className="btn-secondary">Cancel</button>
                <button type="submit" disabled={loading} className="btn-primary">{loading ? 'Saving...' : 'Save Transaction'}</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  )
}
