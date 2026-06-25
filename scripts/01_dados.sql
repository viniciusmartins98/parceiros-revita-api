-- =============================================================================
-- REVITA PARCEIROS — Script de Inserção de Dados Iniciais (Seed)
-- =============================================================================

-- Administrador padrão (senha: admin123 — hash bcrypt placeholder, substituir em produção)
INSERT INTO usuarios (id, nome, email, telefone, senha_hash, perfil, ativo)
VALUES (
    'a0000000-0000-0000-0000-000000000001',
    'Administrador',
    'admin@email.com',
    '00000000000',
    '$2a$12$RtBwh7sObekatm43IIHixeyvuRjFT3MHHZPehE9w5vp0t2VCD2m.C',
    'Administrador',
    TRUE
)
ON CONFLICT (email) DO NOTHING;

-- Regra de pontuação padrão (RN02)
-- R$ 1.000,00 em compras → 100 pontos
-- 100 pontos → R$ 50,00
INSERT INTO regras_pontuacao (
    id,
    nome,
    valor_compra_minimo,
    pontos_por_valor,
    valor_monetario_por_pontos,
    pontos_para_conversao_monetaria,
    ativo,
    criado_por
)
VALUES (
    'b0000000-0000-0000-0000-000000000001',
    'Regra Padrão Revita',
    1000.00,
    100,
    50.00,
    100,
    TRUE,
    'a0000000-0000-0000-0000-000000000001'
)
ON CONFLICT (id) DO NOTHING;

-- =============================================================================
-- FIM DO SCRIPT DE DADOS
-- =============================================================================
