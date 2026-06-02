import matplotlib
matplotlib.use('Agg')
import matplotlib.pyplot as plt
import matplotlib.patches as mpatches
from matplotlib.patches import FancyBboxPatch
import matplotlib.patheffects as pe

plt.rcParams['font.family'] = 'Malgun Gothic'
plt.rcParams['axes.unicode_minus'] = False

# ═══════════════════════════════════════════════════════
# 상수
# ═══════════════════════════════════════════════════════
FIG_W, FIG_H = 26, 22
BOX_W   = 4.0   # 박스 너비
HDR_H   = 0.46  # 헤더 높이
ROW_H   = 0.32  # 행 높이
PAD     = 0.14  # 하단 패딩

# 컬럼 x 좌표 (박스 왼쪽 끝)
C1, C2, C3 = 0.35, 4.70, 9.05       # 마스터 데이터 구역
C4, C5     = 14.50, 19.80           # 유저/플레이 구역

# 행 y 좌표 (박스 상단 끝)
R1, R2, R3, R4 = 20.0, 15.2, 10.0, 4.2

# ═══════════════════════════════════════════════════════
# 색상 팔레트
# ═══════════════════════════════════════════════════════
C = {
    'master': dict(hdr='#1D4ED8', body='#EFF6FF', bd='#60A5FA', sec='#DBEAFE'),
    'user':   dict(hdr='#15803D', body='#F0FDF4', bd='#4ADE80', sec='#DCFCE7'),
    'play':   dict(hdr='#B45309', body='#FFFBEB', bd='#FCD34D', sec='#FEF3C7'),
}

# ═══════════════════════════════════════════════════════
# 테이블 정의  (prefix: 'PK' | 'FK' | '')
# ═══════════════════════════════════════════════════════
TABLES = {
    'elements': {
        'cat': 'master',
        'cols': [
            ('PK', 'element_id',   'INT'),
            ('',   'name',         'VARCHAR(50)  UNIQUE'),
            ('',   'description',  'TEXT'),
        ],
    },
    'characters': {
        'cat': 'master',
        'cols': [
            ('PK', 'character_id',  'INT'),
            ('',   'name',          'VARCHAR(100) UNIQUE'),
            ('FK', 'element_id',    'INT'),
            ('',   'description',   'TEXT'),
            ('',   'sprite_key',    'VARCHAR(255)'),
        ],
    },
    'character_stats': {
        'cat': 'master',
        'cols': [
            ('PK', 'stat_id',       'INT'),
            ('FK', 'character_id',  'INT'),
            ('',   'level',         'INT'),
            ('',   'hp',            'INT'),
            ('',   'atk',           'INT'),
            ('',   'def',           'INT'),
            ('',   'exp_required',  'INT'),
        ],
    },
    'skills': {
        'cat': 'master',
        'cols': [
            ('PK', 'skill_id',     'INT'),
            ('',   'name',         'VARCHAR(100)'),
            ('',   'damage_base',  'INT'),
            ('',   'skill_range',  'FLOAT'),
            ('',   'sp_cost',      'INT'),
            ('FK', 'element_id',   'INT'),
            ('',   'skill_type',   'VARCHAR(20)'),
        ],
    },
    'character_skill_unlocks': {
        'cat': 'master',
        'cols': [
            ('PK', 'unlock_id',    'INT'),
            ('FK', 'character_id', 'INT'),
            ('FK', 'skill_id',     'INT'),
            ('',   'unlock_level', 'INT'),
        ],
    },
    'monster_types': {
        'cat': 'master',
        'cols': [
            ('PK', 'monster_type_id', 'INT'),
            ('',   'name',            'VARCHAR(100)'),
            ('',   'hp',              'INT'),
            ('',   'atk',             'INT'),
            ('',   'def',             'INT'),
            ('',   'exp_drop',        'INT'),
            ('FK', 'element_id',      'INT'),
        ],
    },
    'item_types': {
        'cat': 'master',
        'cols': [
            ('PK', 'item_type_id',  'INT'),
            ('',   'name',          'VARCHAR(100)'),
            ('',   'item_category', 'VARCHAR(20)'),
            ('',   'effect_type',   'VARCHAR(50)'),
            ('',   'effect_value',  'INT'),
        ],
    },
    'dungeons': {
        'cat': 'master',
        'cols': [
            ('PK', 'dungeon_id',   'INT'),
            ('',   'name',         'VARCHAR(100) UNIQUE'),
            ('',   'max_floor',    'INT'),
            ('',   'description',  'TEXT'),
        ],
    },
    'players': {
        'cat': 'user',
        'cols': [
            ('PK', 'player_id',     'INT'),
            ('',   'username',      'VARCHAR(100) UNIQUE'),
            ('',   'password_hash', 'VARCHAR(255)'),
            ('',   'email',         'VARCHAR(255)'),
            ('',   'created_at',    'TIMESTAMPTZ'),
            ('',   'last_login_at', 'TIMESTAMPTZ'),
        ],
    },
    'save_slots': {
        'cat': 'user',
        'cols': [
            ('PK', 'save_slot_id',  'INT'),
            ('FK', 'player_id',     'INT'),
            ('',   'slot_number',   'INT  CHECK(1~3)'),
            ('FK', 'character_id',  'INT'),
            ('',   'current_level', 'INT'),
            ('',   'current_exp',   'INT'),
            ('',   'current_hp',    'INT'),
            ('',   'updated_at',    'TIMESTAMPTZ'),
        ],
    },
    'inventory': {
        'cat': 'user',
        'cols': [
            ('PK', 'inventory_id',  'INT'),
            ('FK', 'save_slot_id',  'INT'),
            ('FK', 'item_type_id',  'INT'),
            ('',   'quantity',      'INT'),
        ],
    },
    'dungeon_sessions': {
        'cat': 'play',
        'cols': [
            ('PK', 'session_id',        'INT'),
            ('FK', 'save_slot_id',      'INT'),
            ('FK', 'dungeon_id',        'INT'),
            ('',   'started_at',        'TIMESTAMPTZ'),
            ('',   'ended_at',          'TIMESTAMPTZ'),
            ('',   'result',            'VARCHAR(20)'),
            ('',   'max_floor_reached', 'INT'),
        ],
    },
    'dungeon_floors': {
        'cat': 'play',
        'cols': [
            ('PK', 'floor_id',      'INT'),
            ('FK', 'session_id',    'INT'),
            ('',   'floor_number',  'INT'),
            ('',   'map_seed',      'INT'),
            ('',   'entered_at',    'TIMESTAMPTZ'),
            ('',   'cleared_at',    'TIMESTAMPTZ'),
            ('',   'total_turns',   'INT'),
        ],
    },
    'battle_logs': {
        'cat': 'play',
        'cols': [
            ('PK', 'log_id',       'INT'),
            ('FK', 'floor_id',     'INT'),
            ('',   'turn_number',  'INT'),
            ('',   'actor_type',   'VARCHAR(10)'),
            ('',   'actor_name',   'VARCHAR(100)'),
            ('',   'action_type',  'VARCHAR(20)'),
            ('',   'target_type',  'VARCHAR(10)'),
            ('',   'damage_dealt', 'INT'),
            ('',   'hp_healed',    'INT'),
        ],
    },
}

# ═══════════════════════════════════════════════════════
# 테이블 위치  (col x, row y)
# ═══════════════════════════════════════════════════════
POS = {
    # ── 마스터 데이터 ─────────────────────────────────
    'elements':                (C2, R1),   # 중앙 상단
    'characters':              (C1, R2),   # 좌
    'skills':                  (C2, R2),   # 중
    'monster_types':           (C3, R2),   # 우
    'character_stats':         (C1, R3),
    'character_skill_unlocks': (C2, R3),
    'item_types':              (C3, R3),
    'dungeons':                (C3, R4),   # 마스터이지만 플레이 구역과 연결
    # ── 유저 / 플레이 데이터 ──────────────────────────
    'players':                 (C4, R1),
    'save_slots':              (C4, R2),
    'inventory':               (C4, R3),
    'dungeon_sessions':        (C5, R2),
    'dungeon_floors':          (C5, R3),
    'battle_logs':             (C5, R4),
}

# ═══════════════════════════════════════════════════════
# 관계 정의  (from, to, label, connection_style, color)
# ═══════════════════════════════════════════════════════
RELS = [
    # elements → 파생 마스터
    ('elements',    'characters',              '1:N', 'arc3,rad=0',    '#64748B'),
    ('elements',    'skills',                  '1:N', 'arc3,rad=0',    '#64748B'),
    ('elements',    'monster_types',           '1:N', 'arc3,rad=0',    '#64748B'),
    # characters 계열
    ('characters',  'character_stats',         '1:N', 'arc3,rad=0',    '#2563EB'),
    ('characters',  'character_skill_unlocks', '1:N', 'arc3,rad=0.18', '#2563EB'),
    ('skills',      'character_skill_unlocks', '1:N', 'arc3,rad=0',    '#2563EB'),
    # item_types → inventory (좌→우 수평)
    ('item_types',  'inventory',               '1:N', 'arc3,rad=0',    '#16A34A'),
    # 플레이어 계열
    ('players',     'save_slots',              '1:N', 'arc3,rad=0',    '#16A34A'),
    # characters → save_slots (구역 횡단, 위로 호)
    ('characters',  'save_slots',              '1:N', 'arc3,rad=-0.28','#7C3AED'),
    # save_slots 하위
    ('save_slots',  'inventory',               '1:N', 'arc3,rad=0',    '#16A34A'),
    ('save_slots',  'dungeon_sessions',        '1:N', 'arc3,rad=0',    '#16A34A'),
    # dungeons → dungeon_sessions (마스터 → 플레이, 대각)
    ('dungeons',    'dungeon_sessions',        '1:N', 'arc3,rad=-0.2', '#B45309'),
    # 플레이 체인
    ('dungeon_sessions', 'dungeon_floors',     '1:N', 'arc3,rad=0',    '#B45309'),
    ('dungeon_floors',   'battle_logs',        '1:N', 'arc3,rad=0',    '#B45309'),
]

# ═══════════════════════════════════════════════════════
# 유틸 함수
# ═══════════════════════════════════════════════════════
def table_h(name):
    return HDR_H + len(TABLES[name]['cols']) * ROW_H + PAD

def anchors(name):
    x, y = POS[name]
    h  = table_h(name)
    cx = x + BOX_W / 2
    cy = y - h / 2
    return {
        'top':    (cx,        y),
        'bottom': (cx,        y - h),
        'left':   (x,         cy),
        'right':  (x + BOX_W, cy),
    }

def pick_side(src, dst):
    sx = POS[src][0] + BOX_W / 2;  sy = POS[src][1] - table_h(src) / 2
    dx = POS[dst][0] + BOX_W / 2;  dy = POS[dst][1] - table_h(dst) / 2
    ddx, ddy = abs(sx - dx), abs(sy - dy)
    if ddx > ddy * 1.2:
        return ('right', 'left') if sx < dx else ('left', 'right')
    return ('bottom', 'top') if sy > dy else ('top', 'bottom')

# ═══════════════════════════════════════════════════════
# 그리기 함수
# ═══════════════════════════════════════════════════════
def draw_section(ax, x, y, w, h, color, label, label_y):
    """구역 배경 패널"""
    ax.add_patch(FancyBboxPatch(
        (x, y), w, h,
        boxstyle='round,pad=0.15', linewidth=1.8,
        edgecolor=color + 'AA', facecolor=color + '28', zorder=1,
    ))
    ax.text(x + w / 2, label_y, label,
            ha='center', va='center', fontsize=10, fontweight='bold',
            color=color,
            bbox=dict(facecolor='white', edgecolor=color + '99',
                      boxstyle='round,pad=0.3', linewidth=1.2), zorder=9)


def draw_table(ax, name):
    x, y  = POS[name]
    h     = table_h(name)
    cat   = TABLES[name]['cat']
    pal   = C[cat]

    # ── 그림자 ──────────────────────────
    ax.add_patch(FancyBboxPatch(
        (x + 0.07, y - h - 0.07), BOX_W, h,
        boxstyle='round,pad=0.06', linewidth=0,
        facecolor='#00000022', zorder=2,
    ))

    # ── 바디 ────────────────────────────
    ax.add_patch(FancyBboxPatch(
        (x, y - h), BOX_W, h,
        boxstyle='round,pad=0.06', linewidth=1.6,
        edgecolor=pal['bd'], facecolor=pal['body'], zorder=3,
    ))

    # ── 헤더 ────────────────────────────
    ax.add_patch(FancyBboxPatch(
        (x, y - HDR_H), BOX_W, HDR_H,
        boxstyle='round,pad=0.06', linewidth=0,
        facecolor=pal['hdr'], zorder=4,
    ))
    ax.text(x + BOX_W / 2, y - HDR_H / 2, name,
            ha='center', va='center', fontsize=9, fontweight='bold',
            color='white', zorder=5)

    # 헤더-바디 구분선
    ax.plot([x + 0.06, x + BOX_W - 0.06], [y - HDR_H, y - HDR_H],
            color=pal['bd'], lw=1.2, zorder=4)

    # ── 컬럼 목록 ───────────────────────
    for i, (pfx, col_name, col_type) in enumerate(TABLES[name]['cols']):
        row_y = y - HDR_H - (i + 0.52) * ROW_H

        if pfx == 'PK':
            pfx_col = '#1D4ED8';  name_col = '#1D4ED8';  fw = 'bold'
        elif pfx == 'FK':
            pfx_col = '#7C3AED';  name_col = '#5B21B6';  fw = 'normal'
        else:
            pfx_col = '#9CA3AF';  name_col = '#1F2937';  fw = 'normal'

        # prefix
        ax.text(x + 0.14, row_y, pfx if pfx else '  ',
                ha='left', va='center', fontsize=6.8,
                color=pfx_col, fontweight=fw,
                fontfamily='monospace', zorder=5)

        # 컬럼명 (굵게)
        ax.text(x + 0.60, row_y, col_name,
                ha='left', va='center', fontsize=7.2,
                color=name_col, fontweight=fw,
                fontfamily='monospace', zorder=5)

        # 타입 (회색)
        ax.text(x + BOX_W - 0.12, row_y, col_type,
                ha='right', va='center', fontsize=6.5,
                color='#6B7280', fontweight='normal',
                fontfamily='monospace', zorder=5)

        # 행 구분선 (마지막 행 제외)
        if i < len(TABLES[name]['cols']) - 1:
            sep_y = y - HDR_H - (i + 1) * ROW_H
            ax.plot([x + 0.06, x + BOX_W - 0.06], [sep_y, sep_y],
                    color=pal['bd'] + '60', lw=0.45,
                    linestyle='--', zorder=3)


def draw_rel(ax, src, dst, label, conn_style, color):
    se, de = pick_side(src, dst)
    sp = anchors(src)[se]
    dp = anchors(dst)[de]

    ax.annotate(
        '', xy=dp, xytext=sp,
        arrowprops=dict(
            arrowstyle='-|>',
            color=color,
            lw=1.4,
            connectionstyle=conn_style,
            shrinkA=4, shrinkB=4,
            mutation_scale=12,
        ),
        zorder=2,
    )

    # 카디널리티 레이블
    mx = (sp[0] + dp[0]) / 2
    my = (sp[1] + dp[1]) / 2
    ax.text(mx, my, label,
            ha='center', va='center', fontsize=7, color=color, fontweight='bold',
            bbox=dict(facecolor='white', edgecolor=color + '99',
                      boxstyle='round,pad=0.18', linewidth=0.9), zorder=8)


# ═══════════════════════════════════════════════════════
# 메인 렌더링
# ═══════════════════════════════════════════════════════
fig, ax = plt.subplots(figsize=(FIG_W, FIG_H))
ax.set_xlim(0, FIG_W)
ax.set_ylim(-0.3, FIG_H)
ax.axis('off')
fig.patch.set_facecolor('#F8FAFC')
ax.set_facecolor('#F8FAFC')

# ── 구역 배경 패널 ──────────────────────────────────────
#  마스터 데이터  (x: C1~C3+BOX_W, y: 2.5~21.1)
draw_section(ax,
    x=0.10, y=2.5, w=C3 + BOX_W + 0.25 - 0.10, h=18.4,
    color='#1D4ED8', label='Master Data', label_y=21.15)

#  유저 데이터  (C4 열)
draw_section(ax,
    x=C4 - 0.25, y=7.5, w=BOX_W + 0.5, h=13.4,
    color='#15803D', label='User Data', label_y=21.15)

#  플레이 기록  (C5 열)
draw_section(ax,
    x=C5 - 0.25, y=0.5, w=BOX_W + 0.5, h=20.4,
    color='#B45309', label='Play Records', label_y=21.15)

# ── 관계선 (테이블 아래) ───────────────────────────────
for src, dst, lbl, conn, col in RELS:
    draw_rel(ax, src, dst, lbl, conn, col)

# ── 테이블 박스 ────────────────────────────────────────
for name in TABLES:
    draw_table(ax, name)

# ── 제목 ───────────────────────────────────────────────
ax.text(FIG_W / 2, FIG_H - 0.15,
        'Artesia  —  Entity Relationship Diagram',
        ha='center', va='top', fontsize=17, fontweight='bold', color='#0F172A',
        path_effects=[pe.withStroke(linewidth=3, foreground='white')])

ax.text(FIG_W / 2, FIG_H - 0.70,
        'PostgreSQL  ·  14 Tables  ·  Master Data / User Data / Play Records',
        ha='center', va='top', fontsize=10, color='#475569')

# ── 범례 ───────────────────────────────────────────────
ax.add_patch(FancyBboxPatch(
    (0.15, 0.15), 5.8, 2.1,
    boxstyle='round,pad=0.1', linewidth=1,
    edgecolor='#CBD5E1', facecolor='white', zorder=8, alpha=0.95))

legend_data = [
    ('#1D4ED8', 'Master Data   (속성·캐릭터·스킬·몬스터·아이템·던전)'),
    ('#15803D', 'User Data      (플레이어·세이브슬롯·인벤토리)'),
    ('#B45309', 'Play Records  (던전세션·층기록·전투로그)'),
]
for i, (col, txt) in enumerate(legend_data):
    yy = 1.85 - i * 0.65
    ax.add_patch(FancyBboxPatch(
        (0.35, yy - 0.18), 0.45, 0.36,
        boxstyle='round,pad=0.04', linewidth=0, facecolor=col, zorder=9))
    ax.text(0.95, yy, txt,
            ha='left', va='center', fontsize=8.2, color='#1E293B', zorder=9)

# ── PK / FK 표기 설명 ───────────────────────────────────
ax.add_patch(FancyBboxPatch(
    (6.3, 0.15), 3.7, 2.1,
    boxstyle='round,pad=0.1', linewidth=1,
    edgecolor='#CBD5E1', facecolor='white', zorder=8, alpha=0.95))
for i, (lbl, col, desc) in enumerate([
    ('PK', '#1D4ED8', '기본 키 (Primary Key)'),
    ('FK', '#7C3AED', '외래 키 (Foreign Key)'),
    ('  ', '#6B7280', '일반 컬럼'),
]):
    yy = 1.85 - i * 0.65
    ax.text(6.55, yy, lbl, ha='left', va='center',
            fontsize=8, fontweight='bold', color=col,
            fontfamily='monospace', zorder=9)
    ax.text(7.00, yy, desc, ha='left', va='center',
            fontsize=8, color='#1E293B', zorder=9)

plt.tight_layout(pad=0.4)
plt.savefig(
    r'C:\Users\sangh\Desktop\Code\Artesia\Artesia_ERD.png',
    dpi=150, bbox_inches='tight',
    facecolor=fig.get_facecolor())
print('saved')
