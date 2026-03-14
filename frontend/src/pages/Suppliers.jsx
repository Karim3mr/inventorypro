import { useEffect, useState } from 'react'
import { supplierService } from '../services/api'
import toast from 'react-hot-toast'
import { Plus, Edit, Trash2, X, Mail, Phone, MapPin } from 'lucide-react'
import { useAuth } from '../context/AuthContext'

const empty = { name: '', contactPerson: '', email: '', phone: '', address: '' }

export default function Suppliers() {
  const { user } = useAuth()
  const canEdit = ['Admin', 'Manager'].includes(user?.role)

  const [suppliers, setSuppliers] = useState([])
  const [showModal, setShowModal] = useState(false)
  const [editing, setEditing] = useState(null)
  const [form, setForm] = useState(empty)

  const load = () => supplierService.getAll().then(r => setSuppliers(r.data))
  useEffect(() => { load() }, [])

  const open = (s = null) => {
    setEditing(s)
    setForm(s ? { name: s.name, contactPerson: s.contactPerson, email: s.email, phone: s.phone, address: s.address } : empty)
    setShowModal(true)
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    if (editing) { await supplierService.update(editing.id, form); toast.success('Supplier updated!') }
    else { await supplierService.create(form); toast.success('Supplier added!') }
    setShowModal(false); load()
  }

  const handleDelete = async (id) => {
    if (!confirm('Deactivate supplier?')) return
    await supplierService.delete(id); toast.success('Deactivated.'); load()
  }

  return (
    <div className="space-y-5">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Suppliers</h1>
          <p className="text-gray-500 text-sm">{suppliers.length} suppliers</p>
        </div>
        {canEdit && <button onClick={() => open()} className="btn-primary flex items-center gap-2"><Plus className="w-4 h-4" />Add Supplier</button>}
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-4">
        {suppliers.map(s => (
          <div key={s.id} className="card space-y-3">
            <div className="flex items-start justify-between">
              <div>
                <h3 className="font-semibold text-gray-900">{s.name}</h3>
                <p className="text-gray-500 text-sm">{s.contactPerson}</p>
              </div>
              <div className="flex items-center gap-1">
                <span className={s.isActive ? 'badge-green' : 'badge-red'}>{s.isActive ? 'Active' : 'Inactive'}</span>
                {canEdit && <button onClick={() => open(s)} className="p-1 text-gray-400 hover:text-blue-600"><Edit className="w-4 h-4" /></button>}
                {user?.role === 'Admin' && <button onClick={() => handleDelete(s.id)} className="p-1 text-gray-400 hover:text-red-600"><Trash2 className="w-4 h-4" /></button>}
              </div>
            </div>
            <div className="space-y-1 text-sm text-gray-600">
              <div className="flex items-center gap-2"><Mail className="w-4 h-4 text-gray-400" />{s.email}</div>
              <div className="flex items-center gap-2"><Phone className="w-4 h-4 text-gray-400" />{s.phone}</div>
              <div className="flex items-center gap-2"><MapPin className="w-4 h-4 text-gray-400" />{s.address}</div>
            </div>
            <div className="pt-1 border-t border-gray-100">
              <span className="badge-blue">{s.productCount} products</span>
            </div>
          </div>
        ))}
      </div>

      {showModal && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 p-4">
          <div className="bg-white rounded-xl shadow-xl w-full max-w-md">
            <div className="flex items-center justify-between px-6 py-4 border-b">
              <h2 className="font-semibold">{editing ? 'Edit Supplier' : 'Add Supplier'}</h2>
              <button onClick={() => setShowModal(false)}><X className="w-5 h-5 text-gray-500" /></button>
            </div>
            <form onSubmit={handleSubmit} className="p-6 space-y-4">
              {[['name', 'Company Name', 'text'], ['contactPerson', 'Contact Person', 'text'], ['email', 'Email', 'email'], ['phone', 'Phone', 'tel'], ['address', 'Address', 'text']].map(([key, label, type]) => (
                <div key={key}>
                  <label className="label">{label} *</label>
                  <input type={type} className="input" required value={form[key]} onChange={e => setForm({ ...form, [key]: e.target.value })} />
                </div>
              ))}
              <div className="flex justify-end gap-3">
                <button type="button" onClick={() => setShowModal(false)} className="btn-secondary">Cancel</button>
                <button type="submit" className="btn-primary">Save</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  )
}
