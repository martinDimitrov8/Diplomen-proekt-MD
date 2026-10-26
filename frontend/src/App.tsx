import { useMemo, useState, type FormEvent, type ReactNode } from 'react';
import {
  Activity, AlertCircle, ArrowDownRight, ArrowUpRight, Bell, CheckCircle2,
  ChevronDown, ChevronRight, CircleHelp, Clock3, LayoutDashboard, Map as MapIcon,
  MapPin, Plus, Search, Settings, ShieldCheck, SlidersHorizontal, Sparkles,
  TrafficCone, TreePine, Trash2, X, Zap,
} from 'lucide-react';

type Status = 'Нов сигнал' | 'В проверка' | 'В процес' | 'Решен';
type IconType = 'road' | 'light' | 'trash' | 'park';
type Report = { id:string; title:string; category:string; address:string; date:string; status:Status; description:string; icon:IconType; color:string; x:number; y:number };
const initialReports: Report[] = [
  {id:'US-0248',title:'Дълбока дупка на пътното платно',category:'Пътища и тротоари',address:'ул. Искър, до № 18',date:'Днес, 10:42',status:'В процес',description:'Има дълбока дупка в дясната лента. Автомобилите рязко сменят посоката си, за да я избегнат.',icon:'road',color:'coral',x:46,y:38},
  {id:'US-0247',title:'Неработещо улично осветление',category:'Улично осветление',address:'парк „Светлина“, северен вход',date:'Днес, 09:18',status:'В проверка',description:'Две лампи не светят от няколко вечери. Участъкът е тъмен след залез.',icon:'light',color:'amber',x:63,y:55},
  {id:'US-0246',title:'Препълнени контейнери за отпадъци',category:'Чистота',address:'бул. „Младост“, спирка 12',date:'Вчера, 17:36',status:'Нов сигнал',description:'Контейнерите са препълнени, а около тях има разпилени отпадъци.',icon:'trash',color:'blue',x:34,y:63},
  {id:'US-0245',title:'Счупена пейка в зелената площ',category:'Паркове и зелени площи',address:'градинка „Орех“, до детската площадка',date:'Вчера, 14:08',status:'Решен',description:'Една от дъските на пейката е счупена. Мястото вече е обезопасено.',icon:'park',color:'green',x:72,y:29},
  {id:'US-0244',title:'Повреден капак на шахта',category:'Пътища и тротоари',address:'ул. „Рила“, кръстовище с ул. „Липа“',date:'20 окт., 16:20',status:'В процес',description:'Капакът е разместен и създава риск за пешеходците.',icon:'road',color:'coral',x:24,y:41},
];
const statusClass: Record<Status,string> = {'Нов сигнал':'status-new','В проверка':'status-review','В процес':'status-progress','Решен':'status-done'};
const iconFor = (type:IconType) => type==='road'?<TrafficCone size={16}/>:type==='light'?<Zap size={16}/>:type==='trash'?<Trash2 size={16}/>:<TreePine size={16}/>;
const views: Record<string,string> = {overview:'Общ преглед',reports:'Всички сигнали',map:'Карта на сигналите',activity:'Актуализации',team:'Служебно табло',settings:'Настройки'};

export default function App() {
  const [reports,setReports] = useState(initialReports);
  const [activeNav,setActiveNav] = useState('overview');
  const [query,setQuery] = useState('');
  const [statusFilter,setStatusFilter] = useState('Всички статуси');
  const [selected,setSelected] = useState<Report|null>(null);
  const [showNew,setShowNew] = useState(false);
  const [showFilters,setShowFilters] = useState(false);
  const [toast,setToast] = useState('');
  const [formTitle,setFormTitle] = useState('');
  const [formCategory,setFormCategory] = useState('Пътища и тротоари');
  const [formAddress,setFormAddress] = useState('');
  const [formDescription,setFormDescription] = useState('');
  const filtered = useMemo(()=>reports.filter(r=>`${r.title} ${r.address} ${r.id} ${r.category}`.toLowerCase().includes(query.toLowerCase()) && (statusFilter==='Всички статуси'||r.status===statusFilter)),[reports,query,statusFilter]);
  const notify = (message:string) => { setToast(message); window.setTimeout(()=>setToast(''),3200); };
  const addDemoReport = (event:FormEvent) => {
    event.preventDefault();
    if(formTitle.trim().length<6||formDescription.trim().length<20||!formAddress.trim()){notify('Попълни заглавие, адрес и описание (минимум 20 знака).');return;}
    const now=new Date();
    const next=249+reports.length-initialReports.length;
    const type:IconType=formCategory.includes('Чистота')?'trash':formCategory.includes('осветление')?'light':formCategory.includes('Паркове')?'park':'road';
    const item:Report={id:`US-${String(next).padStart(4,'0')}`,title:formTitle.trim(),category:formCategory,address:formAddress.trim(),date:`Днес, ${now.toLocaleTimeString('bg-BG',{hour:'2-digit',minute:'2-digit'})}`,status:'Нов сигнал',description:formDescription.trim(),icon:type,color:'blue',x:53,y:47};
    setReports(prev=>[item,...prev]);setSelected(item);setShowNew(false);setFormTitle('');setFormDescription('');setFormAddress('');notify('Добавен е примерен сигнал в текущия прототип.');
  };
  return <div className="app-shell">
    <aside className="sidebar">
      <div className="brand"><div className="brand-mark"><MapPin size={21}/></div><div><strong>urban<span>signals</span></strong><small>Градът, в който живеем</small></div></div>
      <div className="workspace"><div className="workspace-badge">С</div><div className="workspace-copy"><strong>Градска среда</strong><small>Демо пространство</small></div><ChevronDown size={15}/></div>
      <div className="nav-label">РАБОТНО ПРОСТРАНСТВО</div>
      <nav className="nav-list">
        <NavButton active={activeNav==='overview'} onClick={()=>setActiveNav('overview')} icon={<LayoutDashboard/>} label="Общ преглед"/>
        <NavButton active={activeNav==='reports'} onClick={()=>setActiveNav('reports')} icon={<AlertCircle/>} label="Всички сигнали" count={reports.length}/>
        <NavButton active={activeNav==='map'} onClick={()=>setActiveNav('map')} icon={<MapIcon/>} label="Карта на сигналите"/>
        <NavButton active={activeNav==='activity'} onClick={()=>setActiveNav('activity')} icon={<Activity/>} label="Актуализации" dot/>
      </nav>
      <div className="nav-label nav-label-gap">УПРАВЛЕНИЕ</div>
      <nav className="nav-list">
        <NavButton active={activeNav==='team'} onClick={()=>setActiveNav('team')} icon={<ShieldCheck/>} label="Служебно табло"/>
        <NavButton active={activeNav==='settings'} onClick={()=>setActiveNav('settings')} icon={<Settings/>} label="Настройки"/>
      </nav>
      <div className="sidebar-bottom"><div className="help-card"><div className="help-icon"><CircleHelp size={18}/></div><strong>Нуждаеш се от помощ?</strong><p>Виж как работи платформата и как се проследява сигнал.</p><button onClick={()=>notify('Помощният раздел ще бъде реализиран в следващ етап.')}>Център за помощ <ChevronRight size={14}/></button></div><div className="profile"><div className="avatar">МД</div><div className="profile-copy"><strong>Мартин Димитров</strong><small>Демо потребител</small></div><button className="icon-btn" aria-label="Профил" onClick={()=>notify('Профилът и входът ще бъдат добавени в следващ етап.')}><ChevronDown size={16}/></button></div></div>
    </aside>
    <main className="main-area">
      <header className="topbar"><div className="breadcrumb"><span>Градска среда</span><ChevronRight size={14}/><strong>{views[activeNav]}</strong></div><div className="top-actions"><div className="demo-pill"><i/>ДЕМО РЕЖИМ</div><button className="icon-btn notification-btn" aria-label="Известия" onClick={()=>notify('Нямаш нови известия в примерните данни.')}><Bell size={18}/><i/></button><button className="top-avatar" aria-label="Потребител">МД</button></div></header>
      <div className="page-content">
        <section className="page-heading"><div><div className="eyebrow"><Sparkles size={14}/> ПАНЕЛ НА ГРАДСКАТА СРЕДА</div><h1>{activeNav==='overview'?'Добре дошъл, Мартин':views[activeNav]}</h1><p>{activeNav==='overview'?'Ето какво се случва във вашия град днес.':'Прегледай подадените сигнали и текущия им статус.'}</p></div><button className="primary-btn" onClick={()=>setShowNew(true)}><Plus size={18}/> Подай сигнал</button></section>
        <section className="stats-grid">
          <StatCard title="Общо сигнали" value={String(reports.length+243)} note="от началото" icon={<AlertCircle/>} tint="blue" trend="+12%"/>
          <StatCard title="За обработка" value={String(reports.filter(r=>r.status==='Нов сигнал'||r.status==='В проверка').length+18)} note="изискват внимание" icon={<Clock3/>} tint="amber" trend="3 нови"/>
          <StatCard title="В процес" value={String(reports.filter(r=>r.status==='В процес').length+31)} note="работи се по тях" icon={<Activity/>} tint="violet" trend="8 днес"/>
          <StatCard title="Успешно решени" value={String(reports.filter(r=>r.status==='Решен').length+194)} note="проблеми отстранени" icon={<CheckCircle2/>} tint="green" trend="+8.4%"/>
        </section>
        <section className="insight-banner"><div className="insight-symbol"><Sparkles size={19}/></div><div className="insight-copy"><strong>Градът се променя с всеки сигнал.</strong><span>Заедно можем да направим кварталите по-чисти, по-безопасни и по-приятни.</span></div><div className="insight-metric"><span>Средно време за реакция</span><strong>1.8 <small>дни</small></strong><em><ArrowDownRight size={14}/> 12% спрямо предния месец</em></div></section>
        <div className="content-grid">
          <section className="panel map-panel"><div className="panel-head"><div><h2>Сигнали на картата</h2><p>Географски преглед на проблемите</p></div><button className="quiet-btn" onClick={()=>setActiveNav('map')}>Отвори картата <ChevronRight size={15}/></button></div>
            <div className="map-stage"><div className="map-bg"><div className="map-block block-one"/><div className="map-block block-two"/><div className="map-block block-three"/><div className="map-block block-four"/><div className="map-river"/><div className="map-park park-one"/><div className="map-park park-two"/><div className="road road-one"/><div className="road road-two"/><div className="road road-three"/><div className="road road-four"/><div className="road road-five"/><div className="map-label label-one">ЦЕНТЪР</div><div className="map-label label-two">ПАРК СВЕТЛИНА</div><div className="map-label label-three">УЛ. ИСКЪР</div>{reports.slice(0,5).map(r=><button key={r.id} className={`map-pin ${r.color}`} style={{left:`${r.x}%`,top:`${r.y}%`}} aria-label={`Отвори ${r.id}`} onClick={()=>setSelected(r)}>{iconFor(r.icon)}</button>)}<div className="map-controls"><button aria-label="Приближи" onClick={()=>notify('Мащабът на прототипната карта е фиксиран.')}>+</button><button aria-label="Отдалечи" onClick={()=>notify('Мащабът на прототипната карта е фиксиран.')}>−</button></div><div className="map-credit">Схематична карта · примерни данни</div></div></div>
            <div className="map-legend"><span><i className="legend-dot coral"/>Пътища</span><span><i className="legend-dot amber"/>Осветление</span><span><i className="legend-dot blue"/>Чистота</span><span><i className="legend-dot green"/>Зелени площи</span></div>
          </section>
          <section className="panel activity-panel"><div className="panel-head"><div><h2>Последна активност</h2><p>Какво се случва сега</p></div><button className="more-btn" aria-label="Повече" onClick={()=>setActiveNav('activity')}>•••</button></div><div className="timeline">{reports.slice(0,4).map((r,i)=><div className="timeline-item" key={r.id}><div className={`timeline-icon ${r.color}`}>{iconFor(r.icon)}</div><div className="timeline-copy"><strong>{i===0?'Сигналът е в процес на работа':i===1?'Сигналът е изпратен за проверка':i===2?'Нов сигнал е регистриран':'Проблемът е отбелязан като решен'}</strong><span>{r.title}</span><small>{r.date}</small></div><button className="timeline-arrow" aria-label="Детайли" onClick={()=>setSelected(r)}><ChevronRight size={15}/></button></div>)}</div><button className="full-link" onClick={()=>setActiveNav('activity')}>Виж всички актуализации <ChevronRight size={15}/></button></section>
        </div>
        <section className="panel reports-panel"><div className="panel-head report-head"><div><h2>Последни сигнали</h2><p>Следи статуса на проблемите в твоя район</p></div><button className="quiet-btn" onClick={()=>setActiveNav('reports')}>Всички сигнали <ChevronRight size={15}/></button></div>
          <div className="toolbar"><div className="searchbox"><Search size={17}/><input value={query} onChange={e=>setQuery(e.target.value)} placeholder="Търси по сигнал, адрес или номер..."/></div><button className={`filter-btn ${showFilters?'filter-on':''}`} onClick={()=>setShowFilters(!showFilters)}><SlidersHorizontal size={16}/> Филтри <ChevronDown size={14}/></button><span className="result-count">Показани <strong>{filtered.length}</strong> сигнала</span></div>
          {showFilters&&<div className="filter-row"><label>Статус <select value={statusFilter} onChange={e=>setStatusFilter(e.target.value)}><option>Всички статуси</option><option>Нов сигнал</option><option>В проверка</option><option>В процес</option><option>Решен</option></select></label><button className="clear-filter" onClick={()=>{setStatusFilter('Всички статуси');setQuery('');}}>Изчисти филтрите</button></div>}
          <div className="table-scroll"><table><thead><tr><th>СИГНАЛ</th><th>КАТЕГОРИЯ</th><th>ЛОКАЦИЯ</th><th>ДАТА</th><th>СТАТУС</th><th/></tr></thead><tbody>{filtered.map(r=><tr key={r.id} onClick={()=>setSelected(r)}><td><div className={`table-type ${r.color}`}>{iconFor(r.icon)}</div><div className="report-title-cell"><strong>{r.title}</strong><small>{r.id}</small></div></td><td><span className="category-cell">{r.category}</span></td><td><span className="location-cell"><MapPin size={14}/>{r.address}</span></td><td className="date-cell">{r.date}</td><td><span className={`status-badge ${statusClass[r.status]}`}><i/>{r.status}</span></td><td><button className="row-action" aria-label="Детайли" onClick={e=>{e.stopPropagation();setSelected(r);}}><ChevronRight size={16}/></button></td></tr>)}</tbody></table>{filtered.length===0&&<div className="empty-state">Няма сигнали, отговарящи на търсенето.</div>}</div><div className="table-foot"><span>Данните са примерни и се пазят само в текущата сесия.</span><button onClick={()=>setActiveNav('reports')}>Преглед на списъка <ArrowUpRight size={14}/></button></div>
        </section>
        <footer className="footer"><span>© 2026 UrbanSignals · Дипломен проект</span><span><i/> Прототип · не е свързан с общинска система</span><button onClick={()=>notify('Версия на прототипа: 0.1.0')}>Версия 0.1.0</button></footer>
      </div>
    </main>
    {selected&&<div className="modal-backdrop" onClick={()=>setSelected(null)}><section className="detail-modal" onClick={e=>e.stopPropagation()}><button className="modal-close" onClick={()=>setSelected(null)} aria-label="Затвори"><X size={18}/></button><div className={`detail-icon ${selected.color}`}>{iconFor(selected.icon)}</div><div className="detail-id">{selected.id} · {selected.date}</div><h2>{selected.title}</h2><div className="detail-meta"><span className={`status-badge ${statusClass[selected.status]}`}><i/>{selected.status}</span><span>{selected.category}</span></div><p className="detail-description">{selected.description}</p><div className="detail-location"><MapPin size={17}/><div><strong>Местоположение</strong><span>{selected.address}</span></div></div><div className="detail-note"><Clock3 size={16}/><span>Историята на статуса и служебните актуализации ще бъдат свързани с backend в следващ етап.</span></div><button className="primary-btn wide-btn" onClick={()=>{setSelected(null);notify('Проследяването ще бъде свързано с профила в следващ етап.');}}>Проследи сигнала</button></section></div>}
    {showNew&&<div className="modal-backdrop" onClick={()=>setShowNew(false)}><section className="new-modal" onClick={e=>e.stopPropagation()}><div className="modal-heading"><div><span className="eyebrow">НОВ СИГНАЛ</span><h2>Съобщи за проблем</h2><p>Опиши проблема, за да може да бъде разгледан.</p></div><button className="modal-close inline-close" onClick={()=>setShowNew(false)} aria-label="Затвори"><X size={18}/></button></div><form onSubmit={addDemoReport}><label>Заглавие <input value={formTitle} onChange={e=>setFormTitle(e.target.value)} minLength={6} maxLength={120} required placeholder="Напр. Повреден тротоар пред блок 12"/></label><label>Категория <select value={formCategory} onChange={e=>setFormCategory(e.target.value)}><option>Пътища и тротоари</option><option>Чистота</option><option>Улично осветление</option><option>Паркове и зелени площи</option><option>Други</option></select></label><label>Адрес или ориентир <input value={formAddress} onChange={e=>setFormAddress(e.target.value)} required maxLength={200} placeholder="Улица, номер или близък обект"/></label><label>Описание <textarea value={formDescription} onChange={e=>setFormDescription(e.target.value)} minLength={20} maxLength={3000} rows={4} required placeholder="Опиши какво се е случило и какъв е рискът..."/><small className="field-hint">Минимум 20 знака. Прототипът не изпраща данни към сървър.</small></label><div className="form-footer"><button type="button" className="secondary-btn" onClick={()=>setShowNew(false)}>Отказ</button><button className="primary-btn" type="submit"><Plus size={17}/> Добави примерен сигнал</button></div></form></section></div>}
    {toast&&<div role="status" className="toast"><CheckCircle2 size={17}/>{toast}</div>}
  </div>;
}

function NavButton({active,onClick,icon,label,count,dot}:{active:boolean;onClick:()=>void;icon:ReactNode;label:string;count?:number;dot?:boolean}) {
  return <button className={`nav-item ${active?'active':''}`} onClick={onClick}>{icon}<span>{label}</span>{count!==undefined&&<b>{count}</b>}{dot&&<i className="nav-dot"/>}</button>;
}
function StatCard({title,value,note,icon,tint,trend}:{title:string;value:string;note:string;icon:ReactNode;tint:string;trend:string}) {
  return <article className="stat-card"><div className="stat-top"><span>{title}</span><span className={`stat-icon ${tint}`}>{icon}</span></div><div className="stat-value">{value}</div><div className="stat-foot"><span>{note}</span><em className={tint==='amber'?'neutral-trend':''}>{trend}</em></div></article>;
}
