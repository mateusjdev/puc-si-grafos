#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Gerador de Apresentação Acadêmica em PDF (16:9 Widescreen)
Tema: O Algoritmo de Dinic e o Conceito de Fluxo de Bloqueio (Blocking Flow)
Formato: 1280 x 720 pontos (16:9), uma ideia por slide, rigor visual e acadêmico.
"""

import os
import sys
import math
from reportlab.lib.pagesizes import landscape
from reportlab.lib import colors
from reportlab.pdfgen import canvas

# Dimensões 16:9 (720p: 1280 x 720 pt)
W = 1280
H = 720

# Paleta de Cores Acadêmica & Didática
C_NAVY_DARK = colors.HexColor("#0F172A")    # Fundo de Capa / Títulos
C_NAVY_BAR  = colors.HexColor("#1E3A8A")    # Barras e Destaques
C_BG_LIGHT  = colors.HexColor("#F8FAFC")    # Fundo padrão dos slides
C_CARD_BG   = colors.HexColor("#FFFFFF")    # Fundo de cartões
C_CARD_BD   = colors.HexColor("#CBD5E1")    # Borda de cartões
C_TEXT_DARK = colors.HexColor("#1E293B")    # Texto principal escuro
C_TEXT_MUTED= colors.HexColor("#64748B")    # Texto secundário
C_TEXT_LIGHT= colors.HexColor("#F8FAFC")    # Texto claro sobre fundo escuro

# Cores de Grafos (Conforme requisitos)
C_SLACK     = colors.HexColor("#2563EB")    # Azul: Arestas com folga residual
C_SATURATED = colors.HexColor("#DC2626")    # Vermelho: Arestas saturadas
C_PATH      = colors.HexColor("#16A34A")    # Verde: Caminho aumentante atual
C_DISCARDED = colors.HexColor("#94A3B8")    # Cinza: Arestas descartadas / dead-ends
C_LEVEL_BG  = colors.HexColor("#FEF3C7")    # Âmbar claro para badge de nível
C_LEVEL_BD  = colors.HexColor("#D97706")    # Âmbar escuro para borda do nível
C_NODE_BG   = colors.HexColor("#FFFFFF")
C_NODE_BD   = colors.HexColor("#0F172A")

# Posições dos Vértices da Rede de 6 Vértices (Layout Symmetrical Box)
GRAPH_COORDS = {
    "s": (0.0, 0.5),
    "1": (0.33, 0.82),
    "2": (0.33, 0.18),
    "3": (0.67, 0.82),
    "4": (0.67, 0.18),
    "t": (1.0, 0.5),
}

TOTAL_SLIDES = 15

def draw_header_footer(c, slide_num, category, title):
    """Desenha cabeçalho e rodapé padronizados para slides de conteúdo."""
    # Fundo do slide
    c.setFillColor(C_BG_LIGHT)
    c.rect(0, 0, W, H, fill=True, stroke=False)
    
    # Barra superior de acento
    c.setFillColor(C_NAVY_BAR)
    c.rect(0, H - 6, W, 6, fill=True, stroke=False)
    
    # Categoria / Tag (pílula elegante)
    c.setFillColor(colors.HexColor("#DBEAFE"))
    c.roundRect(60, H - 48, 270, 24, 6, fill=True, stroke=False)
    c.setFillColor(C_NAVY_BAR)
    c.setFont("Helvetica-Bold", 11)
    c.drawString(72, H - 41, category.upper())
    
    # Título do Slide (26 pt bold)
    c.setFillColor(C_TEXT_DARK)
    c.setFont("Helvetica-Bold", 26)
    c.drawString(60, H - 84, title)
    
    # Linha divisória discreta
    c.setStrokeColor(colors.HexColor("#E2E8F0"))
    c.setLineWidth(1)
    c.line(60, H - 98, W - 60, H - 98)
    
    # Rodapé
    c.setStrokeColor(colors.HexColor("#E2E8F0"))
    c.line(60, 44, W - 60, 44)
    c.setFont("Helvetica", 12)
    c.setFillColor(C_TEXT_MUTED)
    c.drawString(60, 24, "Teoria e Algoritmos em Grafos • Algoritmo de Dinic & Fluxo de Bloqueio")
    c.drawRightString(W - 60, 24, f"Slide {slide_num} de {TOTAL_SLIDES}")

def draw_card(c, x, y, w, h, bg=C_CARD_BG, border=C_CARD_BD, radius=10):
    """Desenha um cartão com bordas arredondadas e sombra sutil."""
    # Sombra suave
    c.setFillColor(colors.HexColor("#00000008"))
    c.roundRect(x + 2, y - 2, w, h, radius, fill=True, stroke=False)
    # Cartão
    c.setFillColor(bg)
    c.setStrokeColor(border)
    c.setLineWidth(1.2)
    c.roundRect(x, y, w, h, radius, fill=True, stroke=True)

def draw_bullet_points(c, x, y_start, items, line_gap=24, item_gap=16, font_size=18, max_w=480):
    """
    Renderiza lista de tópicos acadêmicos com quebra de linha inteligente (word wrap),
    respeitando corpo >= 18pt, indentação elegante e largura máxima do cartão.
    """
    current_y = y_start
    bullet_indent = 24
    
    for item in items:
        # Extrai prefixo em negrito (se houver ':')
        if ":" in item:
            parts = item.split(":", 1)
            prefix = parts[0] + ":"
            rest = parts[1].strip()
        else:
            prefix = ""
            rest = item.strip()
            
        words = rest.split()
        lines = []
        
        if prefix:
            prefix_w = c.stringWidth(prefix + " ", "Helvetica-Bold", font_size)
            curr_w = prefix_w
            first_words = []
            idx = 0
            while idx < len(words):
                w = words[idx]
                word_w = c.stringWidth(w + " ", "Helvetica", font_size)
                if curr_w + word_w > max_w and first_words:
                    break
                first_words.append(w)
                curr_w += word_w
                idx += 1
            lines.append((prefix, " ".join(first_words)))
            
            # Linhas seguintes
            curr_words = []
            curr_w = 0
            while idx < len(words):
                w = words[idx]
                word_w = c.stringWidth(w + " ", "Helvetica", font_size)
                if curr_w + word_w > max_w and curr_words:
                    lines.append(("", " ".join(curr_words)))
                    curr_words = [w]
                    curr_w = word_w
                else:
                    curr_words.append(w)
                    curr_w += word_w
                idx += 1
            if curr_words:
                lines.append(("", " ".join(curr_words)))
        else:
            curr_words = []
            curr_w = 0
            for w in words:
                word_w = c.stringWidth(w + " ", "Helvetica", font_size)
                if curr_w + word_w > max_w and curr_words:
                    lines.append(("", " ".join(curr_words)))
                    curr_words = [w]
                    curr_w = word_w
                else:
                    curr_words.append(w)
                    curr_w += word_w
            if curr_words:
                lines.append(("", " ".join(curr_words)))
                
        # Marcador circular azul na primeira linha do item
        c.setFillColor(C_NAVY_BAR)
        c.circle(x + 7, current_y + font_size * 0.35, 4, fill=True, stroke=False)
        
        # Desenhar cada linha
        for line_idx, (pfx, txt) in enumerate(lines):
            line_y = current_y - line_idx * line_gap
            draw_x = x + bullet_indent
            if pfx:
                c.setFont("Helvetica-Bold", font_size)
                c.setFillColor(C_TEXT_DARK)
                c.drawString(draw_x, line_y, pfx)
                pfx_w = c.stringWidth(pfx + " ", "Helvetica-Bold", font_size)
                draw_x += pfx_w
                
            c.setFont("Helvetica", font_size)
            c.setFillColor(C_TEXT_DARK)
            c.drawString(draw_x, line_y, txt)
            
        current_y -= len(lines) * line_gap + item_gap

def draw_graph(c, box_x, box_y, box_w, box_h, nodes_levels, edges, active_path=None, title="Grafo Residual / Níveis"):
    """
    Desenha diagrama nítido da rede com vértices circulares, rótulos de nível e arestas coloridas.
    """
    draw_card(c, box_x, box_y, box_w, box_h, bg=colors.white, border=C_CARD_BD, radius=12)
    
    # Título do diagrama
    c.setFillColor(C_NAVY_BAR)
    c.setFont("Helvetica-Bold", 15)
    c.drawString(box_x + 20, box_y + box_h - 26, title)
    
    # Área interna do grafo
    pad_x = 55
    pad_y = 45
    g_x = box_x + pad_x
    g_y = box_y + pad_y
    g_w = box_w - 2 * pad_x
    g_h = box_h - pad_y - 45
    
    R = 22 # Raio do vértice
    node_pos = {}
    for name, (nx, ny) in GRAPH_COORDS.items():
        px = g_x + nx * g_w
        py = g_y + ny * g_h
        node_pos[name] = (px, py)
        
    path_edges = set()
    if active_path:
        for i in range(len(active_path) - 1):
            path_edges.add((active_path[i], active_path[i+1]))
            
    # Desenhar Arestas
    for u, v, flow, cap, state in edges:
        x1, y1 = node_pos[u]
        x2, y2 = node_pos[v]
        is_in_path = (u, v) in path_edges
        
        if is_in_path:
            edge_color = C_PATH
            edge_width = 3.5
        elif state == "saturated":
            edge_color = C_SATURATED
            edge_width = 2.6
        elif state == "slack":
            edge_color = C_SLACK
            edge_width = 2.2
        else: # discarded
            edge_color = C_DISCARDED
            edge_width = 1.4
            
        dx = x2 - x1
        dy = y2 - y1
        dist = math.hypot(dx, dy)
        if dist == 0:
            continue
        ux = dx / dist
        uy = dy / dist
        
        is_curved = (u == "2" and v == "1") or (u == "4" and v == "3")
        curve_offset = 24 if is_curved else 0
        
        sx = x1 + ux * R
        sy = y1 + uy * R
        ex = x2 - ux * (R + 6)
        ey = y2 - uy * (R + 6)
        
        c.setStrokeColor(edge_color)
        c.setFillColor(edge_color)
        c.setLineWidth(edge_width)
        
        if is_curved:
            nx = -uy * curve_offset
            ny = ux * curve_offset
            cx = (x1 + x2) / 2 + nx
            cy = (y1 + y2) / 2 + ny
            
            p = c.beginPath()
            p.moveTo(sx, sy)
            p.curveTo((sx + cx)/2, (sy + cy)/2, (ex + cx)/2, (ey + cy)/2, ex, ey)
            c.drawPath(p, stroke=True, fill=False)
            
            lbl_x = (x1 + x2)/2 + nx * 0.9
            lbl_y = (y1 + y2)/2 + ny * 0.9
            arr_ux = (ex - cx) / math.hypot(ex - cx, ey - cy)
            arr_uy = (ey - cy) / math.hypot(ex - cx, ey - cy)
        else:
            c.line(sx, sy, ex, ey)
            lbl_x = (x1 + x2) / 2
            lbl_y = (y1 + y2) / 2
            arr_ux = ux
            arr_uy = uy
            
        # Ponta da seta
        arrow_len = 10
        arrow_w = 6
        tip_x = ex + arr_ux * 6
        tip_y = ey + arr_uy * 6
        base_x = tip_x - arr_ux * arrow_len
        base_y = tip_y - arr_uy * arrow_len
        left_x = base_x - arr_uy * arrow_w
        left_y = base_y + arr_ux * arrow_w
        right_x = base_x + arr_uy * arrow_w
        right_y = base_y - arr_ux * arrow_w
        
        arrow_path = c.beginPath()
        arrow_path.moveTo(tip_x, tip_y)
        arrow_path.lineTo(left_x, left_y)
        arrow_path.lineTo(right_x, right_y)
        arrow_path.close()
        c.drawPath(arrow_path, stroke=False, fill=True)
        
        # Pílula com rótulo
        lbl_str = f"{flow}/{cap}"
        c.setFont("Helvetica-Bold", 10)
        str_w = c.stringWidth(lbl_str, "Helvetica-Bold", 10)
        
        c.setFillColor(colors.white)
        c.setStrokeColor(edge_color)
        c.setLineWidth(1)
        c.roundRect(lbl_x - str_w/2 - 4, lbl_y - 7, str_w + 8, 14, 3, fill=True, stroke=True)
        c.setFillColor(edge_color)
        c.drawCentredString(lbl_x, lbl_y - 3.5, lbl_str)
        
    # Desenhar Vértices
    for name, (px, py) in node_pos.items():
        lvl = nodes_levels.get(name, None)
        
        c.setFillColor(colors.HexColor("#00000010"))
        c.circle(px + 1.5, py - 1.5, R, fill=True, stroke=False)
        
        c.setFillColor(C_NODE_BG)
        c.setStrokeColor(C_NODE_BD)
        c.setLineWidth(2.2)
        c.circle(px, py, R, fill=True, stroke=True)
        
        c.setFillColor(C_TEXT_DARK)
        c.setFont("Helvetica-Bold", 16)
        c.drawCentredString(px, py - 5.5, name)
        
        # Badge de Nível
        if lvl is not None:
            lvl_str = f"L={lvl}" if lvl != -1 else "L=∞"
            c.setFillColor(C_LEVEL_BG)
            c.setStrokeColor(C_LEVEL_BD)
            c.setLineWidth(1)
            c.roundRect(px - 16, py + R - 4, 32, 14, 4, fill=True, stroke=True)
            c.setFillColor(C_LEVEL_BD)
            c.setFont("Helvetica-Bold", 9)
            c.drawCentredString(px, py + R - 0.5, lvl_str)
            
    # Legenda compacta
    leg_y = box_y + 12
    c.setFont("Helvetica", 10)
    c.setFillColor(C_SLACK)
    c.circle(box_x + 30, leg_y + 3, 4, fill=True, stroke=False)
    c.setFillColor(C_TEXT_DARK)
    c.drawString(box_x + 38, leg_y, "Folga Residual")
    
    c.setFillColor(C_SATURATED)
    c.circle(box_x + 130, leg_y + 3, 4, fill=True, stroke=False)
    c.setFillColor(C_TEXT_DARK)
    c.drawString(box_x + 138, leg_y, "Saturada")
    
    if active_path:
        c.setFillColor(C_PATH)
        c.circle(box_x + 210, leg_y + 3, 4, fill=True, stroke=False)
        c.setFillColor(C_TEXT_DARK)
        c.drawString(box_x + 218, leg_y, "Caminho Atual")
        
    c.setFillColor(C_LEVEL_BD)
    c.drawString(box_x + box_w - 95, leg_y, "L = Nível (BFS)")


# ==============================================================================
# DEFINIÇÃO DOS 15 SLIDES
# ==============================================================================

def slide_01_capa(c):
    """Slide 1: Capa Acadêmica Formal"""
    c.setFillColor(C_NAVY_DARK)
    c.rect(0, 0, W, H, fill=True, stroke=False)
    
    c.setFillColor(colors.HexColor("#3B82F6"))
    c.rect(0, H - 8, W, 8, fill=True, stroke=False)
    
    c.setFillColor(colors.HexColor("#1E293B"))
    c.setStrokeColor(colors.HexColor("#334155"))
    c.roundRect(80, 80, W - 160, H - 160, 16, fill=True, stroke=True)
    
    c.setFillColor(colors.HexColor("#38BDF8"))
    c.setFont("Helvetica-Bold", 14)
    c.drawString(130, H - 160, "TEORIA E ALGORITMOS EM GRAFOS • ATIVIDADE PRÁTICA")
    
    c.setFillColor(colors.white)
    c.setFont("Helvetica-Bold", 38)
    c.drawString(130, H - 230, "O Algoritmo de Dinic")
    c.setFont("Helvetica-Bold", 34)
    c.setFillColor(colors.HexColor("#93C5FD"))
    c.drawString(130, H - 280, "& o Conceito de Fluxo de Bloqueio")
    
    c.setFillColor(colors.HexColor("#CBD5E1"))
    c.setFont("Helvetica", 20)
    c.drawString(130, H - 335, "Fundamentos Teóricos, Mecânica de Níveis e Execução Passo a Passo")
    
    c.setStrokeColor(colors.HexColor("#475569"))
    c.setLineWidth(1.5)
    c.line(130, H - 370, W - 130, H - 370)
    
    c.setFont("Helvetica-Bold", 16)
    c.setFillColor(colors.HexColor("#38BDF8"))
    c.drawString(130, H - 420, "DISCIPLINA:")
    c.setFont("Helvetica", 18)
    c.setFillColor(colors.white)
    c.drawString(260, H - 420, "Teoria dos Grafos / Algoritmos em Grafos")
    
    c.setFont("Helvetica-Bold", 16)
    c.setFillColor(colors.HexColor("#38BDF8"))
    c.drawString(130, H - 460, "INTEGRANTES:")
    c.setFont("Helvetica", 18)
    c.setFillColor(colors.white)
    c.drawString(260, H - 460, "[Nomes dos Integrantes / Equipe do Projeto]")
    
    c.setFont("Helvetica-Bold", 16)
    c.setFillColor(colors.HexColor("#38BDF8"))
    c.drawString(130, H - 500, "REFERÊNCIA:")
    c.setFont("Helvetica", 18)
    c.setFillColor(colors.white)
    c.drawString(260, H - 500, "Implementação em C# baseada em SuperGrafoDirecionado")
    
    c.setFont("Helvetica-Oblique", 14)
    c.setFillColor(colors.HexColor("#94A3B8"))
    c.drawRightString(W - 130, 115, "Novembro de 2026")

def slide_02_motivacao(c):
    """Slide 2: Motivação e Contexto Histórico"""
    draw_header_footer(c, 2, "Contexto Histórico & Motivação", "Por que precisamos do Algoritmo de Dinic?")
    
    # Coluna 1
    draw_card(c, 60, 90, 560, 480)
    c.setFillColor(C_SATURATED)
    c.setFont("Helvetica-Bold", 20)
    c.drawString(90, 525, "Gargalos dos Algoritmos Clássicos")
    
    bullets_col1 = [
        "Ford-Fulkerson (1956): Arbitrário na escolha de rotas, dependendo do valor do fluxo máximo com complexidade O(E·|f*|).",
        "Edmonds-Karp (1972): Usa BFS para encontrar a menor rota aumentante por vez, atingindo O(V·E²).",
        "O Custo Redundante: A cada único caminho encontrado, Edmonds-Karp reconstrói uma BFS completa do zero.",
        "Trabalho Desperdiçado: Milhares de buscas BFS repetidas para caminhos que possuem o mesmo comprimento residual."
    ]
    draw_bullet_points(c, 90, 470, bullets_col1, line_gap=24, item_gap=16, font_size=18, max_w=480)
    
    # Coluna 2
    draw_card(c, 660, 90, 560, 480, bg=colors.HexColor("#F0FDF4"), border=colors.HexColor("#BBF7D0"))
    c.setFillColor(C_PATH)
    c.setFont("Helvetica-Bold", 20)
    c.drawString(690, 525, "A Revolução do Algoritmo de Dinic (1970)")
    
    bullets_col2 = [
        "Ideia Fundamental: Em vez de achar um caminho por BFS, aproveite toda a camada de distância mínima!",
        "Grafo de Níveis (Level Graph): Constrói uma visão acíclica das menores rotas s → t via BFS.",
        "Fluxo de Bloqueio (Blocking Flow): Satura múltiplos caminhos em lote usando DFS com ponteiros.",
        "Complexidade Superior: Cai para O(V²·E), e até O(E·√V) em redes unitárias, superando amplamente Edmonds-Karp."
    ]
    draw_bullet_points(c, 690, 470, bullets_col2, line_gap=24, item_gap=16, font_size=18, max_w=480)

def slide_03_conceitos_base(c):
    """Slide 3: Fundamentos Teóricos da Rede Residual"""
    draw_header_footer(c, 3, "Fundamentos de Redes de Fluxo", "Rede Residual, Capacidades e Conservação")
    
    draw_card(c, 60, 90, 1160, 480)
    c.setFillColor(C_NAVY_BAR)
    c.setFont("Helvetica-Bold", 22)
    c.drawString(90, 525, "Os Pilares da Formulação de Fluxo em Redes")
    
    bullets = [
        "Capacidade Residual (cf): Quantidade adicional que pode ser enviada por uma aresta na rede residual. Para a aresta direta: cf(u, v) = c(u, v) - f(u, v).",
        "Arestas Reversas Virtuais: Para cada aresta direta (u, v), existe uma aresta reversa (v, u) com cf(v, u) = f(u, v). Permite cancelar ou redirecionar fluxo anterior.",
        "Caminho Aumentante: Uma rota direcionada simples de s até t em Gf onde toda aresta intermediária possui capacidade residual estritamente positiva (cf > 0).",
        "Fluxo de Gargalo (Bottleneck): A capacidade residual mínima ao longo de um caminho aumentante: Δ = min { cf(u, v) : (u, v) ∈ P }.",
        "Conservação e Capacidade: Em todo vértice intermediário, soma do fluxo entrante = soma do fluxo sainte; e 0 ≤ f(e) ≤ c(e)."
    ]
    draw_bullet_points(c, 90, 470, bullets, line_gap=24, item_gap=16, font_size=18, max_w=1080)
    
    # Caixa de Destaque no Rodapé
    c.setFillColor(colors.HexColor("#EFF6FF"))
    c.setStrokeColor(colors.HexColor("#BFDBFE"))
    c.roundRect(90, 115, 1100, 65, 8, fill=True, stroke=True)
    c.setFillColor(C_NAVY_BAR)
    c.setFont("Helvetica-Bold", 17)
    c.drawString(115, 150, "Teorema Fundamental do Fluxo Máximo e Corte Mínimo (Ford-Fulkerson):")
    c.setFont("Helvetica", 16)
    c.setFillColor(C_TEXT_DARK)
    c.drawString(115, 128, "Um fluxo f é máximo em G se, e somente se, a rede residual Gf não contém nenhum caminho aumentante de s para t.")

def slide_04_grafo_niveis(c):
    """Slide 4: O Grafo de Níveis (Level Graph)"""
    draw_header_footer(c, 4, "Estrutura Central", "O Grafo de Níveis (Level Graph GL)")
    
    draw_card(c, 60, 90, 520, 480)
    c.setFillColor(C_NAVY_BAR)
    c.setFont("Helvetica-Bold", 20)
    c.drawString(90, 525, "Construção por Busca em Largura (BFS)")
    
    bullets = [
        "Definição de Nível: Para cada vértice v, nivel[v] é a distância mínima (em saltos) de s até v no grafo residual.",
        "Inicialização: Define-se nivel[s] = 0 e nivel[v] = -1 para todos os demais vértices da rede.",
        "Critério de Admissibilidade em GL: A aresta residual (u, v) pertence a GL se, e somente se: cf(u, v) > 0  e  nivel[v] = nivel[u] + 1.",
        "Natureza Acíclica: O grafo de níveis GL é estritamente um DAG, orientando o fluxo sempre para frente.",
        "Condição de Parada: Se a BFS não alcançar t (nivel[t] == -1), o algoritmo termina: fluxo máximo alcançado!"
    ]
    draw_bullet_points(c, 90, 470, bullets, line_gap=24, item_gap=16, font_size=18, max_w=460)
    
    sample_edges = [
        ("s", "1", 0, 10, "slack"),
        ("s", "2", 0, 10, "slack"),
        ("1", "3", 0, 4, "slack"),
        ("1", "4", 0, 8, "slack"),
        ("2", "4", 0, 9, "slack"),
        ("3", "t", 0, 10, "slack"),
        ("4", "t", 0, 10, "slack"),
        ("2", "1", 0, 2, "discarded"),
        ("4", "3", 0, 6, "discarded")
    ]
    sample_levels = {"s": 0, "1": 1, "2": 1, "3": 2, "4": 2, "t": 3}
    draw_graph(c, 600, 90, 620, 480, sample_levels, sample_edges, active_path=None, title="Grafo de Níveis Inicial (Fase 1)")

def slide_05_definicao_fluxo_bloqueio(c):
    """Slide 5: Definição de Fluxo de Bloqueio (Conceito Chave)"""
    draw_header_footer(c, 5, "Conceito Central", "O que é um Fluxo de Bloqueio (Blocking Flow)?")
    
    draw_card(c, 60, 90, 1160, 480)
    c.setFillColor(C_NAVY_BAR)
    c.setFont("Helvetica-Bold", 22)
    c.drawString(90, 525, "Definição Rigorosa e Distinção Essencial")
    
    bullets = [
        "Definição Formal: Um fluxo f' no grafo de níveis GL é dito um Fluxo de Bloqueio se todo caminho direcionado de s a t em GL contém pelo menos uma aresta saturada (onde f'(u, v) = cf(u, v)).",
        "Esgotamento Completo: Após empurrar o fluxo de bloqueio, não resta nenhum caminho s → t direcionado dentro daquele grafo de níveis específico.",
        "Diferença Crucial: Fluxo de Bloqueio NÃO é o Fluxo Máximo! Ele satura apenas as rotas de menor comprimento da fase. Não precisa ser máximo nem mesmo em GL.",
        "Crescimento Estrito da Distância: Após aplicar um fluxo de bloqueio e recalcular a BFS, a nova distância dist(s, t) estritamente aumenta: dist_{k+1}(s, t) > dist_k(s, t).",
        "Limite Superior de Fases: Como o caminho simples mais longo tem |V| - 1 arestas, o algoritmo executa no máximo |V| - 1 fases de bloqueio!"
    ]
    draw_bullet_points(c, 90, 470, bullets, line_gap=24, item_gap=16, font_size=18, max_w=1080)
    
    c.setFillColor(colors.HexColor("#FEF3C7"))
    c.setStrokeColor(colors.HexColor("#FDE68A"))
    c.roundRect(90, 115, 1100, 68, 8, fill=True, stroke=True)
    c.setFillColor(colors.HexColor("#B45309"))
    c.setFont("Helvetica-Bold", 17)
    c.drawString(115, 153, "Atenção Teórica (Distinção de Conceito):")
    c.setFont("Helvetica", 16)
    c.setFillColor(C_TEXT_DARK)
    c.drawString(115, 130, "Enquanto um Fluxo Máximo bloqueia caminhos em toda a rede residual Gf, o Fluxo de Bloqueio bloqueia caminhos apenas no subgrafo de níveis GL.")

def slide_06_calculo_fluxo_bloqueio(c):
    """Slide 6: Mecânica de Cálculo do Fluxo de Bloqueio (DFS e Ponteiros)"""
    draw_header_footer(c, 6, "Mecânica do Algoritmo", "Como Calcular o Fluxo de Bloqueio com DFS e Ponteiros")
    
    # Coluna 1
    draw_card(c, 60, 90, 560, 480)
    c.setFillColor(C_NAVY_BAR)
    c.setFont("Helvetica-Bold", 20)
    c.drawString(90, 525, "O Vetor de Ponteiros ptr[] (Current-Arc)")
    
    bullets_col1 = [
        "Problema da DFS Ingênua: Percorrer arestas que já saturaram ou que não chegam a t custaria tempo repetitivo desnecessário.",
        "Ponteiro ptr[u]: Cada vértice u mantém um índice apontando para a sua próxima aresta válida na lista de adjacência.",
        "Avanço sem Retrocesso: Se a aresta adj[u][ptr[u]] não consegue enviar mais fluxo, incrementa-se ptr[u]++. Ele nunca retrocede na mesma fase!",
        "Eliminação de Dead-Ends: Vértices sem rota para o sumidouro são descartados permanentemente para o resto da fase.",
        "Custo por Fase: Cada aresta é inspecionada um número constante de vezes, garantindo tempo O(V·E) para toda a fase de bloqueio!"
    ]
    draw_bullet_points(c, 90, 470, bullets_col1, line_gap=24, item_gap=16, font_size=18, max_w=480)
    
    # Coluna 2
    draw_card(c, 660, 90, 560, 480, bg=colors.HexColor("#F8FAFC"), border=colors.HexColor("#CBD5E1"))
    c.setFillColor(C_PATH)
    c.setFont("Helvetica-Bold", 20)
    c.drawString(690, 525, "Ciclo Recursivo de Aumento de Fluxo")
    
    bullets_col2 = [
        "1. Busca em Profundidade: Avança de u para v somente se nivel[v] == nivel[u] + 1 e cf(u, v) > 0.",
        "2. Identificação do Gargalo: Ao atingir o sumidouro t, retorna o fluxo empurrável Δ = min(fluxo, cf).",
        "3. Atualização Residual: Na volta da recursão, subtrai-se Δ da aresta direta e soma-se Δ na aresta reversa.",
        "4. Loop até Bloquear: Repete-se a DFS a partir de s até que ela retorne 0 unidades de fluxo.",
        "5. Bloqueio Atingido: A soma de todos os Δ enviados nesta fase é o Fluxo de Bloqueio daquela camada!"
    ]
    draw_bullet_points(c, 690, 470, bullets_col2, line_gap=24, item_gap=16, font_size=18, max_w=480)

def slide_07_apresentacao_rede(c):
    """Slide 7: Apresentação da Rede de Exemplo"""
    draw_header_footer(c, 7, "Rede de Teste de Referência", "Rede de Fluxo com 6 Vértices e Múltiplas Fases")
    
    draw_card(c, 60, 90, 520, 480)
    c.setFillColor(C_NAVY_BAR)
    c.setFont("Helvetica-Bold", 20)
    c.drawString(90, 525, "Especificação da Rede do Exemplo")
    
    bullets = [
        "Vértices: V = { s, 1, 2, 3, 4, t } (6 vértices). Fonte: s, Sumidouro: t.",
        "Arestas da Fonte: s → 1 (cap 10),  s → 2 (cap 10).",
        "Arestas Internas Superiores: 1 → 3 (cap 4),  1 → 4 (cap 8).",
        "Arestas Transversais: 2 → 1 (cap 2),  4 → 3 (cap 6).",
        "Arestas para o Sumidouro: 2 → 4 (cap 9),  3 → t (cap 10),  4 → t (cap 10).",
        "Meta de Execução: Exige exatamente 3 fases completas de BFS + Fluxo de Bloqueio para convergir ao Fluxo Máximo = 20."
    ]
    draw_bullet_points(c, 90, 470, bullets, line_gap=24, item_gap=16, font_size=18, max_w=460)
    
    init_edges = [
        ("s", "1", 0, 10, "slack"),
        ("s", "2", 0, 10, "slack"),
        ("1", "3", 0, 4, "slack"),
        ("1", "4", 0, 8, "slack"),
        ("2", "1", 0, 2, "slack"),
        ("2", "4", 0, 9, "slack"),
        ("3", "t", 0, 10, "slack"),
        ("4", "3", 0, 6, "slack"),
        ("4", "t", 0, 10, "slack")
    ]
    init_levels = {"s": 0, "1": 1, "2": 1, "3": 2, "4": 2, "t": 3}
    draw_graph(c, 600, 90, 620, 480, init_levels, init_edges, active_path=None, title="Rede de Fluxo Inicial (Capacidades e Fluxo Zero)")

def slide_08_fase_1(c):
    """Slide 8: Execução Dinic - Fase 1"""
    draw_header_footer(c, 8, "Execução Passo a Passo", "Fase 1: Grafo de Níveis com Distância d = 3")
    
    draw_card(c, 60, 90, 520, 480)
    c.setFillColor(C_NAVY_BAR)
    c.setFont("Helvetica-Bold", 20)
    c.drawString(90, 525, "Caminhos Aumentantes da Fase 1")
    
    bullets = [
        "Grafo de Níveis: s(0) → {1, 2}(1) → {3, 4}(2) → t(3). Distância s–t = 3.",
        "Caminho 1: s → 1 → 3 → t  | Gargalo = 4. Satura aresta 1 → 3. ptr[3] avança.",
        "Caminho 2: s → 1 → 4 → t  | Gargalo = 6. Satura aresta s → 1. ptr[1] avança.",
        "Caminho 3: s → 2 → 4 → t  | Gargalo = 4. Satura aresta 4 → t. ptr[4] avança.",
        "Fluxo de Bloqueio da Fase 1: 4 + 6 + 4 = 14 unidades.",
        "Fluxo Acumulado: 14. Todos os caminhos de nível 3 foram bloqueados!"
    ]
    draw_bullet_points(c, 90, 470, bullets, line_gap=24, item_gap=16, font_size=18, max_w=460)
    
    fase1_edges = [
        ("s", "1", 10, 10, "saturated"),
        ("s", "2", 4, 10, "slack"),
        ("1", "3", 4, 4, "saturated"),
        ("1", "4", 6, 8, "slack"),
        ("2", "1", 0, 2, "discarded"),
        ("2", "4", 4, 9, "slack"),
        ("3", "t", 4, 10, "slack"),
        ("4", "3", 0, 6, "discarded"),
        ("4", "t", 10, 10, "saturated")
    ]
    fase1_levels = {"s": 0, "1": 1, "2": 1, "3": 2, "4": 2, "t": 3}
    draw_graph(c, 600, 90, 620, 480, fase1_levels, fase1_edges, active_path=["s", "2", "4", "t"], title="Rede Residual ao Final da Fase 1 (Fluxo = 14)")

def slide_09_fase_2(c):
    """Slide 9: Execução Dinic - Fase 2"""
    draw_header_footer(c, 9, "Execução Passo a Passo", "Fase 2: Grafo de Níveis com Distância d = 4")
    
    draw_card(c, 60, 90, 520, 480)
    c.setFillColor(C_NAVY_BAR)
    c.setFont("Helvetica-Bold", 20)
    c.drawString(90, 525, "Caminhos Aumentantes da Fase 2")
    
    bullets = [
        "Nova BFS Residual: Como s → 1 e 4 → t saturaram, a menor rota s–t agora tem 4 arestas!",
        "Grafo de Níveis: s(0) → 2(1) → 4(2) → 3(3) → t(4). Vértice 1 tem nível 2 via 2 → 1.",
        "Caminho 1: s → 2 → 4 → 3 → t  | Gargalo = min(6, 5, 6, 6) = 5.",
        "Saturação: A aresta 2 → 4 atinge seu limite (9/9) e satura!",
        "Fluxo de Bloqueio da Fase 2: 5 unidades enviadas.",
        "Fluxo Acumulado: 14 + 5 = 19 unidades. Todos os caminhos de nível 4 estão bloqueados!"
    ]
    draw_bullet_points(c, 90, 470, bullets, line_gap=24, item_gap=16, font_size=18, max_w=460)
    
    fase2_edges = [
        ("s", "1", 10, 10, "saturated"),
        ("s", "2", 9, 10, "slack"),
        ("1", "3", 4, 4, "saturated"),
        ("1", "4", 6, 8, "slack"),
        ("2", "1", 0, 2, "slack"),
        ("2", "4", 9, 9, "saturated"),
        ("3", "t", 9, 10, "slack"),
        ("4", "3", 5, 6, "slack"),
        ("4", "t", 10, 10, "saturated")
    ]
    fase2_levels = {"s": 0, "1": 2, "2": 1, "3": 3, "4": 2, "t": 4}
    draw_graph(c, 600, 90, 620, 480, fase2_levels, fase2_edges, active_path=["s", "2", "4", "3", "t"], title="Rede Residual ao Final da Fase 2 (Fluxo = 19)")

def slide_10_fase_3_termino(c):
    """Slide 10: Execução Dinic - Fase 3 e Término"""
    draw_header_footer(c, 10, "Execução Passo a Passo", "Fase 3 (d = 5) e Critério de Parada")
    
    draw_card(c, 60, 90, 520, 480)
    c.setFillColor(C_NAVY_BAR)
    c.setFont("Helvetica-Bold", 20)
    c.drawString(90, 525, "Fase 3 & Convergência Final")
    
    bullets = [
        "Nova BFS Residual: Distância s–t aumenta para 5! s(0) → 2(1) → 1(2) → 4(3) → 3(4) → t(5).",
        "Caminho 1: s → 2 → 1 → 4 → 3 → t  | Gargalo = min(1, 2, 2, 1, 1) = 1.",
        "Saturações Simultâneas: Satura s → 2 (10/10), 4 → 3 (6/6) e 3 → t (10/10)!",
        "Fluxo de Bloqueio da Fase 3: 1 unidade.",
        "Fluxo Máximo Atingido: 14 + 5 + 1 = 20 unidades.",
        "Fase 4 (Condição de Parada): Nova BFS não alcança nenhum vértice a partir de s. nivel[t] = -1. Algoritmo encerrado!"
    ]
    draw_bullet_points(c, 90, 470, bullets, line_gap=24, item_gap=16, font_size=18, max_w=460)
    
    fase3_edges = [
        ("s", "1", 10, 10, "saturated"),
        ("s", "2", 10, 10, "saturated"),
        ("1", "3", 4, 4, "saturated"),
        ("1", "4", 7, 8, "slack"),
        ("2", "1", 1, 2, "slack"),
        ("2", "4", 9, 9, "saturated"),
        ("3", "t", 10, 10, "saturated"),
        ("4", "3", 6, 6, "saturated"),
        ("4", "t", 10, 10, "saturated")
    ]
    fase3_levels = {"s": 0, "1": 2, "2": 1, "3": 4, "4": 3, "t": 5}
    draw_graph(c, 600, 90, 620, 480, fase3_levels, fase3_edges, active_path=["s", "2", "1", "4", "3", "t"], title="Rede Residual Final com Fluxo Máximo = 20")

def slide_11_pseudocodigo(c):
    """Slide 11: Pseudocódigo Estruturado de Dinic"""
    draw_header_footer(c, 11, "Especificação Algorítmica", "Pseudocódigo do Algoritmo de Dinic")
    
    # Caixa 1: Laço Principal e BFS
    draw_card(c, 60, 90, 560, 480, bg=colors.HexColor("#F8FAFC"), border=colors.HexColor("#CBD5E1"))
    c.setFillColor(C_NAVY_BAR)
    c.setFont("Helvetica-Bold", 16)
    c.drawString(85, 535, "Algoritmo Principal & Construção de Níveis (BFS)")
    
    code_bfs = [
        "funcao Dinic(G, s, t):",
        "    fluxo_total = 0",
        "    enquanto BFS_ConstruirNiveis(G, s, t) faca:",
        "        ptr = vetor_zeros(V)",
        "        enquanto (emp = DFS_Bloqueio(s, t, INF, ptr)) > 0:",
        "            fluxo_total += emp",
        "    retorne fluxo_total",
        "",
        "funcao BFS_ConstruirNiveis(G, s, t):",
        "    nivel = vetor_preenchido(V, -1); nivel[s] = 0",
        "    fila.enfileirar(s)",
        "    enquanto fila_nao_vazia():",
        "        u = fila.desenfileirar()",
        "        para cada aresta (u, v) em Gf:",
        "            se cf(u, v) > 0 e nivel[v] == -1:",
        "                nivel[v] = nivel[u] + 1",
        "                fila.enfileirar(v)",
        "    retorne nivel[t] != -1"
    ]
    c.setFont("Courier-Bold", 11)
    py = 512
    for line in code_bfs:
        if line.startswith("funcao"):
            c.setFillColor(colors.HexColor("#1D4ED8"))
        elif line.strip().startswith("enquanto") or line.strip().startswith("se") or line.strip().startswith("retorne"):
            c.setFillColor(colors.HexColor("#B91C1C"))
        else:
            c.setFillColor(colors.HexColor("#0F172A"))
        c.drawString(85, py, line)
        py -= 18.5
        
    # Caixa 2: DFS com Ponteiro ptr[]
    draw_card(c, 660, 90, 560, 480, bg=colors.HexColor("#F8FAFC"), border=colors.HexColor("#CBD5E1"))
    c.setFillColor(C_PATH)
    c.setFont("Helvetica-Bold", 16)
    c.drawString(685, 535, "Fluxo de Bloqueio com Ponteiros ptr[] (DFS)")
    
    code_dfs = [
        "funcao DFS_Bloqueio(u, t, fluxo_disp, ptr):",
        "    se u == t ou fluxo_disp == 0: retorne fluxo_disp",
        "    // ptr[u] avanca e NUNCA retrocede na fase",
        "    enquanto ptr[u] < grau_saida(u):",
        "        aresta = adj[u][ptr[u]]; v = aresta.destino",
        "        se nivel[v] == nivel[u] + 1 e aresta.cf > 0:",
        "            gargalo = min(fluxo_disp, aresta.cf)",
        "            emp = DFS_Bloqueio(v, t, gargalo, ptr)",
        "            se emp > 0:",
        "                aresta.fluxo += emp",
        "                aresta.reversa.fluxo -= emp",
        "                retorne emp",
        "        ptr[u] = ptr[u] + 1  // Dead-end ou saturada",
        "    retorne 0"
    ]
    c.setFont("Courier-Bold", 11)
    py = 512
    for line in code_dfs:
        if line.startswith("funcao"):
            c.setFillColor(colors.HexColor("#1D4ED8"))
        elif line.strip().startswith("se") or line.strip().startswith("enquanto") or line.strip().startswith("retorne"):
            c.setFillColor(colors.HexColor("#B91C1C"))
        elif "//" in line:
            c.setFillColor(colors.HexColor("#059669"))
        else:
            c.setFillColor(colors.HexColor("#0F172A"))
        c.drawString(685, py, line)
        py -= 18.5

def slide_12_implementacao_csharp(c):
    """Slide 12: Destaques da Implementação em C#"""
    draw_header_footer(c, 12, "Engenharia de Software", "Destaques da Implementação em C#")
    
    draw_card(c, 60, 90, 520, 480)
    c.setFillColor(C_NAVY_BAR)
    c.setFont("Helvetica-Bold", 20)
    c.drawString(90, 525, "Integração com a Estrutura Existente")
    
    bullets = [
        "Compatibilidade Total: Consome diretamente as interfaces IGrafo e IGrafoPonderado e as classes SuperGrafoDirecionado e Grafo.",
        "Rede Residual Encapsulada: Classe interna ArestaResidual mantém ponteiro direto para sua aresta Reversa, eliminando buscas O(E).",
        "Otimização ptr[]: Vetor inteiro alocado uma única vez por fase, avançando em tempo constante O(1).",
        "Robustez por Identificador: Dicionários indexados por IVertice com VerticeEqualityComparer evitam armadilhas de igualdade por referência.",
        "Relatório Rico: Registro formal de cada fase em FaseDinic e cálculo automático do Corte Mínimo (S, T)."
    ]
    draw_bullet_points(c, 90, 470, bullets, line_gap=24, item_gap=16, font_size=18, max_w=460)
    
    # Coluna 2: Código C# Real
    draw_card(c, 600, 90, 620, 480, bg=colors.HexColor("#0F172A"), border=colors.HexColor("#1E293B"))
    c.setFillColor(colors.HexColor("#38BDF8"))
    c.setFont("Helvetica-Bold", 14)
    c.drawString(625, 535, "Trecho Real: DfsFluxoBloqueio (Dinic.cs)")
    
    csharp_code = [
        "for (; ptr[u] < adj[u].Count; ptr[u]++) {",
        "    var aresta = adj[u][ptr[u]];",
        "    int v = aresta.DestinoIndex;",
        "",
        "    // Apenas arestas no grafo de niveis com capacidade residual",
        "    if (niveis[v] == niveis[u] + 1 && aresta.CapacidadeResidual > 1e-9) {",
        "        double gargalo = Math.Min(fluxoDisponivel, aresta.CapacidadeResidual);",
        "        double empurrado = DfsFluxoBloqueio(v, t, gargalo, niveis, ptr, adj);",
        "",
        "        if (empurrado > 1e-9) {",
        "            aresta.Fluxo += empurrado;",
        "            aresta.Reversa.Fluxo -= empurrado;",
        "            return empurrado; // Retorna fluxo aumentante",
        "        }",
        "    }",
        "}",
        "return 0; // Vertice u esgotado nesta fase"
    ]
    c.setFont("Courier", 11.5)
    py = 495
    for line in csharp_code:
        if "for" in line or "if" in line or "return" in line or "var" in line or "double" in line or "int" in line:
            c.setFillColor(colors.HexColor("#F472B6"))
        elif "//" in line:
            c.setFillColor(colors.HexColor("#94A3B8"))
        else:
            c.setFillColor(colors.HexColor("#E2E8F0"))
        c.drawString(625, py, line)
        py -= 22

def slide_13_complexidade(c):
    """Slide 13: Análise Teórica de Complexidade"""
    draw_header_footer(c, 13, "Análise de Complexidade", "Demonstração Teórica e Comparativo de Desempenho")
    
    draw_card(c, 60, 90, 560, 480)
    c.setFillColor(C_NAVY_BAR)
    c.setFont("Helvetica-Bold", 20)
    c.drawString(90, 525, "Demonstração: Por que O(V²·E)?")
    
    bullets = [
        "1. No máximo V - 1 Fases: A cada fluxo de bloqueio aplicado, a distância dist(s, t) cresce no mínimo em 1 unidade. Como o maior caminho simples tem V - 1 arestas, há no máximo V fases.",
        "2. Custo por Fase O(V·E): A cada caminho aumentante encontrado, ao menos uma aresta é saturada (no máximo E vezes). O custo de alcançar o gargalo é O(V), logo O(V·E).",
        "3. Custo dos Avanços de ptr[]: Cada aresta do grafo é incrementada em ptr no máximo 1 vez por fase, somando O(E).",
        "4. Custo Total: (V fases) × O(V·E por fase) = O(V²·E)."
    ]
    draw_bullet_points(c, 90, 470, bullets, line_gap=24, item_gap=16, font_size=18, max_w=480)
    
    # Coluna 2: Tabela Comparativa
    draw_card(c, 660, 90, 560, 480)
    c.setFillColor(C_NAVY_BAR)
    c.setFont("Helvetica-Bold", 20)
    c.drawString(690, 525, "Comparativo com Outros Algoritmos")
    
    tbl_x = 690
    tbl_y = 445
    row_h = 42
    headers = ["Algoritmo", "Geral", "Redes Unitárias"]
    col_w = [180, 150, 170]
    
    c.setFillColor(C_NAVY_BAR)
    c.rect(tbl_x, tbl_y, sum(col_w), row_h, fill=True, stroke=False)
    c.setFillColor(colors.white)
    c.setFont("Helvetica-Bold", 14)
    cx = tbl_x + 10
    for h, w in zip(headers, col_w):
        c.drawString(cx, tbl_y + 14, h)
        cx += w
        
    data = [
        ("Ford-Fulkerson", "O(E · |f*|)", "O(V · E)"),
        ("Edmonds-Karp", "O(V · E²)", "O(V · E²)"),
        ("Dinic (Geral)", "O(V² · E)", "O(E · √V)"),
        ("Dinic (Bipartido)", "O(V² · E)", "O(E · √V) [Hopcroft]"),
        ("Push-Relabel (Tarjan)", "O(V³)", "O(V² · √E)")
    ]
    
    for i, row in enumerate(data):
        curr_y = tbl_y - (i + 1) * row_h
        bg_col = colors.HexColor("#EFF6FF") if "Dinic" in row[0] else (colors.HexColor("#F8FAFC") if i % 2 == 0 else colors.white)
        c.setFillColor(bg_col)
        c.rect(tbl_x, curr_y, sum(col_w), row_h, fill=True, stroke=False)
        c.setFillColor(C_NAVY_BAR if "Dinic" in row[0] else C_TEXT_DARK)
        c.setFont("Helvetica-Bold" if "Dinic" in row[0] else "Helvetica", 14)
        cx = tbl_x + 10
        for val, w in zip(row, col_w):
            c.drawString(cx, curr_y + 14, val)
            cx += w

def slide_14_resultados(c):
    """Slide 14: Resultados Numéricos e Corte Mínimo"""
    draw_header_footer(c, 14, "Resultados Numéricos", "Validação Experimental e Corte Mínimo (S, T)")
    
    draw_card(c, 60, 90, 560, 480)
    c.setFillColor(C_NAVY_BAR)
    c.setFont("Helvetica-Bold", 20)
    c.drawString(90, 525, "Estado Final das Arestas da Rede")
    
    tbl_x = 90
    tbl_y = 445
    row_h = 36
    col_w = [140, 110, 110, 140]
    headers = ["Aresta", "Fluxo", "Capacidade", "Status"]
    
    c.setFillColor(C_NAVY_BAR)
    c.rect(tbl_x, tbl_y, sum(col_w), row_h, fill=True, stroke=False)
    c.setFillColor(colors.white)
    c.setFont("Helvetica-Bold", 13)
    cx = tbl_x + 10
    for h, w in zip(headers, col_w):
        c.drawString(cx, tbl_y + 12, h)
        cx += w
        
    edge_rows = [
        ("s → 1", "10", "10", "SATURADA [CORTE]"),
        ("s → 2", "10", "10", "SATURADA [CORTE]"),
        ("1 → 3", "4", "4", "SATURADA"),
        ("1 → 4", "7", "8", "Folga Residual = 1"),
        ("2 → 1", "1", "2", "Folga Residual = 1"),
        ("2 → 4", "9", "9", "SATURADA"),
        ("3 → t", "10", "10", "SATURADA"),
        ("4 → 3", "6", "6", "SATURADA"),
        ("4 → t", "10", "10", "SATURADA"),
    ]
    
    for i, row in enumerate(edge_rows):
        curr_y = tbl_y - (i + 1) * row_h
        bg_col = colors.HexColor("#FEF2F2") if "CORTE" in row[3] else (colors.HexColor("#F8FAFC") if i % 2 == 0 else colors.white)
        c.setFillColor(bg_col)
        c.rect(tbl_x, curr_y, sum(col_w), row_h, fill=True, stroke=False)
        c.setFont("Helvetica-Bold" if "CORTE" in row[3] else "Helvetica", 13)
        c.setFillColor(C_SATURATED if "CORTE" in row[3] else C_TEXT_DARK)
        cx = tbl_x + 10
        for val, w in zip(row, col_w):
            c.drawString(cx, curr_y + 11, val)
            cx += w
            
    # Coluna 2: Análise do Corte Mínimo com Word Wrap
    draw_card(c, 660, 90, 560, 480, bg=colors.HexColor("#EFF6FF"), border=colors.HexColor("#BFDBFE"))
    c.setFillColor(C_NAVY_BAR)
    c.setFont("Helvetica-Bold", 20)
    c.drawString(690, 525, "Dualidade: Fluxo Máximo = Corte Mínimo")
    
    bullets_corte = [
        "Partição S (Alcançável da fonte em Gf): S = { s }.",
        "Partição T (Inalcançável): T = { 1, 2, 3, 4, t }.",
        "Arestas que Cruzam o Corte (S → T):",
        "  • Aresta s → 1 com capacidade 10 (Saturada)",
        "  • Aresta s → 2 com capacidade 10 (Saturada)",
        "Capacidade do Corte Mínimo: c(S, T) = 10 + 10 = 20.",
        "Verificação Numérica: Fluxo Máximo (20) == Capacidade do Corte (20). O resultado gerado pelo C# é 100% exato e comprovado!"
    ]
    draw_bullet_points(c, 690, 470, bullets_corte, line_gap=24, item_gap=16, font_size=18, max_w=480)

def slide_15_conclusao_referencias(c):
    """Slide 15: Aplicações Práticas, Conclusão e Referências"""
    draw_header_footer(c, 15, "Conclusão & Bibliografia", "Aplicações Práticas e Referências Bibliográficas")
    
    draw_card(c, 60, 90, 560, 480)
    c.setFillColor(C_NAVY_BAR)
    c.setFont("Helvetica-Bold", 20)
    c.drawString(90, 525, "Aplicações Reais de Fluxo de Bloqueio")
    
    bullets = [
        "Emparelhamento Bipartido: O algoritmo de Hopcroft-Karp é uma especialização direta de Dinic em redes unitárias (tempo O(E·√V)).",
        "Visão Computacional: O problema de corte mínimo em grafos (Graph Cuts) para segmentação de imagens usa variantes de Dinic.",
        "Logística e Roteamento: Otimização de vazão em redes rodoviárias, elétricas e de telecomunicações.",
        "Conclusão: O fluxo de bloqueio é um paradigma que revolucionou a otimização ao decompor o problema em camadas acíclicas independentes."
    ]
    draw_bullet_points(c, 90, 470, bullets, line_gap=24, item_gap=16, font_size=18, max_w=480)
    
    draw_card(c, 660, 90, 560, 480, bg=colors.HexColor("#F8FAFC"), border=colors.HexColor("#CBD5E1"))
    c.setFillColor(C_NAVY_BAR)
    c.setFont("Helvetica-Bold", 20)
    c.drawString(690, 525, "Referências Bibliográficas Clássicas")
    
    refs = [
        ("Dinitz, Yefim (1970):", "'Algorithm for solution of a problem of maximal flow in a network with power estimation'. Soviet Mathematics Doklady, 11: 1277–1280."),
        ("Cormen, T. H. et al. (2009):", "'Algoritmos: Teoria e Prática'. 3ª Edição, Campus/Elsevier, Capítulo 26 (Fluxo Máximo)."),
        ("Even, S. & Tarjan, R. E. (1975):", "'Network Flow and Testing Graph Connectivity'. SIAM Journal on Computing, 4(4): 507–518."),
        ("Ahuja, R. K. et al. (1993):", "'Network Flows: Theory, Algorithms, and Applications'. Prentice Hall.")
    ]
    
    ry = 470
    for title, desc in refs:
        c.setFillColor(C_NAVY_BAR)
        c.circle(690 + 7, ry + 6, 4, fill=True, stroke=False)
        c.setFont("Helvetica-Bold", 16)
        c.setFillColor(C_TEXT_DARK)
        c.drawString(690 + 24, ry, title)
        
        words = desc.split()
        curr_words = []
        curr_w = 0
        desc_lines = []
        for w in words:
            word_w = c.stringWidth(w + " ", "Helvetica", 15)
            if curr_w + word_w > 480 and curr_words:
                desc_lines.append(" ".join(curr_words))
                curr_words = [w]
                curr_w = word_w
            else:
                curr_words.append(w)
                curr_w += word_w
        if curr_words:
            desc_lines.append(" ".join(curr_words))
            
        c.setFont("Helvetica", 15)
        c.setFillColor(C_TEXT_MUTED)
        for i_line, line_str in enumerate(desc_lines):
            c.drawString(690 + 24, ry - 20 - i_line * 20, line_str)
            
        ry -= (20 + len(desc_lines) * 20 + 16)

def build_pdf(filename="dinic_fluxo_de_bloqueio.pdf"):
    """Gera o arquivo PDF final com os 15 slides formatados."""
    c = canvas.Canvas(filename, pagesize=(W, H))
    
    slide_funcs = [
        slide_01_capa,
        slide_02_motivacao,
        slide_03_conceitos_base,
        slide_04_grafo_niveis,
        slide_05_definicao_fluxo_bloqueio,
        slide_06_calculo_fluxo_bloqueio,
        slide_07_apresentacao_rede,
        slide_08_fase_1,
        slide_09_fase_2,
        slide_10_fase_3_termino,
        slide_11_pseudocodigo,
        slide_12_implementacao_csharp,
        slide_13_complexidade,
        slide_14_resultados,
        slide_15_conclusao_referencias
    ]
    
    for i, func in enumerate(slide_funcs, start=1):
        func(c)
        c.showPage()
        
    c.save()
    print(f"PDF gerado com sucesso: {filename}")

if __name__ == "__main__":
    output_pdf = sys.argv[1] if len(sys.argv) > 1 else "dinic_fluxo_de_bloqueio.pdf"
    build_pdf(output_pdf)
