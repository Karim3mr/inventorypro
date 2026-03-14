import { useEffect, useState } from 'react'
import { categoryService } from '../services/api'
import toast from 'react-hot-toast'
import { Plus, Edit, Trash2, X } from 'lucide-react'
import { useAuth } from '../context/AuthContext'

export default function Categories() {
  const { user } = useAuth()
  const canEdit = ['Admin', 'Manager'].includes(user?.role)
  const isAdmin = user?.role === 'Admin'

  const [categories, setCategories] = useState([])
  const [showModal, setShowModal] = useState(false)
  const [editing, setEditing] = useState(null)
  const [form, setForm] = useState({ name: '', description: '' })

  const load = () => categoryService.getAll().then(r => setCategories(r.data))
  useEffect(() => { load() }, [])

  const open = (cat = null) => {
    setEditing(cat)
    setForm(cat ? { name: cat.name, description: cat.description || '' } : { name: '', description: '' })
    setShowModal(true)
  }

  const handleSubmit = async (e) => {
    e.preventDefault()
    if (editing) {
      await categoryService.update(editing.id, form)
      toast.success('Category updated!')
    } else {
      await categoryService.create(form)
      toast.success('Category created!')
    }
    setShowModal(false); load()
  }

  const handleDelete = async (id) => {
    if (!confirm('Delete this category?')) return
    await categoryService.delete(id)
    toast.success('Deleted.'); load()
  }

  return (
    <div className="space-y-5">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Categories</h1>
          <p className="text-gray-500 text-sm">{categories.length} categories</p>
        </div>
        {canEdit && <button onClick={() => open()} className="btn-primary flex items-center gap-2"><Plus className="w-4 h-4" />Add Category</button>}
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
        {categories.map(c => (
          <div key={c.id} className="card flex items-start justify-between">
            <div>
              <h3 className="font-semibold text-gray-900">{c.name}</h3>
              <p className="text-gray-500 text-sm mt-1">{c.description || 'No description'}</p>
              <span className="badge-blue mt-2">{c.productCount} products</span>
            </div>
            <div className="flex gap-1">
              {canEdit && <button onClick={() => open(c)} className="p-1.5 text-gray-400 hover:text-blue-600"><Edit className="w-4 h-4" /></button>}
              {isAdmin && <button onClick={() => handleDelete(c.id)} className="p-1.5 text-gray-400 hover:text-red-600"><Trash2 className="w-4 h-4" /></button>}
            </div>
          </div>
        ))}
      </div>

      {showModal && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 p-4">
          <div className="bg-white rounded-xl shadow-xl w-full max-w-md">
            <div className="flex items-center justify-between px-6 py-4 border-b">
              <h2 className="font-semibold">{editing ? 'Edit Category' : 'Add Category'}</h2>
              <button onClick={() => setShowModal(false)}><X className="w-5 h-5 text-gray-500" /></button>
            </div>
            <form onSubmit={handleSubmit} className="p-6 space-y-4">
              <div>
                <label className="label">Name *</label>
                <input className="input" required value={form.name} onChange={e => setForm({ ...form, name: e.target.value })} />
              </div>
              <div>
                <label className="label">Description</label>
                <textarea className="input" rows={3} value={form.description} onChange={e => setForm({ ...form, description: e.target.value })} />
              </div>
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
