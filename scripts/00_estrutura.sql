-- =============================================================================
-- REVITA PARCEIROS — Script de Criação do Banco de Dados (PostgreSQL)
-- =============================================================================
-- Descrição:  Cria o schema completo do sistema Revita Parceiros (Estrutura).
-- Requisitos: PostgreSQL 15+
-- =============================================================================

-- Habilita extensões necessárias
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE EXTENSION IF NOT EXISTS "pgcrypto";

-- =============================================================================
-- 1. TABELA: usuarios
--    Armazena todos os usuários do sistema (Administrador, Funcionario, Parceiro, Cliente).
-- =============================================================================

CREATE TABLE IF NOT EXISTS usuarios (
    id              UUID            PRIMARY KEY DEFAULT uuid_generate_v4(),
    nome            VARCHAR(200)    NOT NULL,
    email           VARCHAR(254)    NULL,
    telefone        VARCHAR(20)     NULL,
    senha_hash      TEXT            NOT NULL,
    perfil          VARCHAR(50)     NOT NULL,
    ativo           BOOLEAN         NOT NULL DEFAULT TRUE,
    criado_em       TIMESTAMPTZ     NOT NULL DEFAULT NOW(),
    atualizado_em   TIMESTAMPTZ     NULL,

    -- Constraints
    CONSTRAINT uq_usuarios_email   UNIQUE (email)
);

COMMENT ON TABLE  usuarios                IS 'Tabela unificada de usuários do sistema (Administrador, Funcionario, Parceiro, Cliente).';
COMMENT ON COLUMN usuarios.perfil         IS 'Perfil de acesso: Administrador, Funcionario, Parceiro, Cliente.';
COMMENT ON COLUMN usuarios.senha_hash     IS 'Hash da senha gerado via bcrypt/Argon2.';
COMMENT ON COLUMN usuarios.telefone       IS 'Telefone/WhatsApp do usuário.';

-- Índices
CREATE INDEX IF NOT EXISTS ix_usuarios_perfil       ON usuarios (perfil);
CREATE INDEX IF NOT EXISTS ix_usuarios_email        ON usuarios (email)      WHERE email IS NOT NULL;
CREATE INDEX IF NOT EXISTS ix_usuarios_ativo        ON usuarios (ativo)      WHERE ativo = TRUE;

-- =============================================================================
-- 2. TABELA: funcionarios
--    Dados complementares do perfil Funcionario.
-- =============================================================================

CREATE TABLE IF NOT EXISTS funcionarios (
    id              UUID            PRIMARY KEY DEFAULT uuid_generate_v4(),
    usuario_id      UUID            NOT NULL,
    criado_em       TIMESTAMPTZ     NOT NULL DEFAULT NOW(),
    atualizado_em   TIMESTAMPTZ     NULL,

    -- FK
    CONSTRAINT fk_funcionarios_usuario
        FOREIGN KEY (usuario_id) REFERENCES usuarios (id) ON DELETE RESTRICT,

    -- Unique
    CONSTRAINT uq_funcionarios_usuario UNIQUE (usuario_id)
);

COMMENT ON TABLE funcionarios IS 'Dados complementares do Funcionario (vinculado ao usuario).';

-- =============================================================================
-- 3. TABELA: parceiros
--    Dados complementares do perfil Parceiro.
-- =============================================================================

CREATE TABLE IF NOT EXISTS parceiros (
    id              UUID            PRIMARY KEY DEFAULT uuid_generate_v4(),
    usuario_id      UUID            NOT NULL,
    total_pontos    INTEGER         NOT NULL DEFAULT 0,
    criado_em       TIMESTAMPTZ     NOT NULL DEFAULT NOW(),
    atualizado_em   TIMESTAMPTZ     NULL,

    -- FK
    CONSTRAINT fk_parceiros_usuario
        FOREIGN KEY (usuario_id) REFERENCES usuarios (id) ON DELETE RESTRICT,

    -- Unique
    CONSTRAINT uq_parceiros_usuario UNIQUE (usuario_id),

    -- Check
    CONSTRAINT ck_parceiros_total_pontos CHECK (total_pontos >= 0)
);

COMMENT ON TABLE  parceiros              IS 'Dados complementares do Parceiro — saldo de pontos acumulado.';
COMMENT ON COLUMN parceiros.total_pontos IS 'Saldo atual de pontos do parceiro (ganhos - resgates).';

-- =============================================================================
-- 4. TABELA: clientes
--    Dados complementares do perfil Cliente (consumidor final).
-- =============================================================================

CREATE TABLE IF NOT EXISTS clientes (
    id              UUID            PRIMARY KEY DEFAULT uuid_generate_v4(),
    usuario_id      UUID            NOT NULL,
    criado_em       TIMESTAMPTZ     NOT NULL DEFAULT NOW(),
    atualizado_em   TIMESTAMPTZ     NULL,

    -- FK
    CONSTRAINT fk_clientes_usuario
        FOREIGN KEY (usuario_id) REFERENCES usuarios (id) ON DELETE RESTRICT,

    -- Unique
    CONSTRAINT uq_clientes_usuario UNIQUE (usuario_id)
);

COMMENT ON TABLE  clientes             IS 'Dados complementares do Cliente (consumidor final).';

-- =============================================================================
-- 5. TABELA: compras
--    Registro de compras realizadas por clientes na loja.
-- =============================================================================

CREATE TABLE IF NOT EXISTS compras (
    id                  UUID            PRIMARY KEY DEFAULT uuid_generate_v4(),
    cliente_id          UUID            NOT NULL,
    parceiro_id         UUID            NOT NULL,
    registrado_por      UUID            NOT NULL,
    valor               NUMERIC(12,2)   NOT NULL,
    numero_cupom        VARCHAR(50)     NULL,
    data_compra         TIMESTAMPTZ     NOT NULL DEFAULT NOW(),
    pontos_gerados      INTEGER         NOT NULL DEFAULT 0,
    criado_em           TIMESTAMPTZ     NOT NULL DEFAULT NOW(),
    atualizado_em       TIMESTAMPTZ     NULL,

    -- FK
    CONSTRAINT fk_compras_cliente
        FOREIGN KEY (cliente_id) REFERENCES clientes (id) ON DELETE RESTRICT,

    CONSTRAINT fk_compras_parceiro
        FOREIGN KEY (parceiro_id) REFERENCES parceiros (id) ON DELETE RESTRICT,

    CONSTRAINT fk_compras_registrado_por
        FOREIGN KEY (registrado_por) REFERENCES usuarios (id) ON DELETE RESTRICT,

    -- Check
    CONSTRAINT ck_compras_valor           CHECK (valor > 0),
    CONSTRAINT ck_compras_pontos_gerados  CHECK (pontos_gerados >= 0)
);

COMMENT ON TABLE  compras                   IS 'Lançamentos de compras feitas por clientes na loja.';
COMMENT ON COLUMN compras.registrado_por    IS 'Funcionário ou Administrador que lançou a compra.';
COMMENT ON COLUMN compras.numero_cupom      IS 'Número do cupom fiscal (opcional).';
COMMENT ON COLUMN compras.pontos_gerados    IS 'Pontos gerados para o parceiro nesta compra.';

-- Índices
CREATE INDEX IF NOT EXISTS ix_compras_cliente         ON compras (cliente_id);
CREATE INDEX IF NOT EXISTS ix_compras_parceiro        ON compras (parceiro_id);
CREATE INDEX IF NOT EXISTS ix_compras_registrado_por  ON compras (registrado_por);
CREATE INDEX IF NOT EXISTS ix_compras_data_compra     ON compras (data_compra DESC);

-- =============================================================================
-- 6. TABELA: extrato_pontos (Ledger de Pontos — Imutável / Auditoria)
--    Registra toda geração e resgate de pontos (RNF04).
-- =============================================================================

CREATE TABLE IF NOT EXISTS extrato_pontos (
    id                  UUID                    PRIMARY KEY DEFAULT uuid_generate_v4(),
    parceiro_id         UUID                    NOT NULL,
    tipo_transacao      VARCHAR(50)             NOT NULL,
    pontos              INTEGER                 NOT NULL,
    valor_monetario     NUMERIC(12,2)           NULL,
    compra_id           UUID                    NULL,
    descricao           VARCHAR(500)            NULL,
    criado_em           TIMESTAMPTZ             NOT NULL DEFAULT NOW(),

    -- FK
    CONSTRAINT fk_extrato_parceiro
        FOREIGN KEY (parceiro_id) REFERENCES parceiros (id) ON DELETE RESTRICT,

    CONSTRAINT fk_extrato_compra
        FOREIGN KEY (compra_id) REFERENCES compras (id) ON DELETE RESTRICT,

    -- Check
    CONSTRAINT ck_extrato_pontos CHECK (pontos != 0)
);

COMMENT ON TABLE  extrato_pontos                  IS 'Ledger imutável — registra toda movimentação de pontos (geração e resgate).';
COMMENT ON COLUMN extrato_pontos.tipo_transacao   IS 'Tipo: Ganho (gerado por compra) ou Resgate (resgate pelo parceiro).';
COMMENT ON COLUMN extrato_pontos.valor_monetario  IS 'Valor em R$ equivalente aos pontos (para resgates).';
COMMENT ON COLUMN extrato_pontos.compra_id        IS 'Compra que originou os pontos (NULL para resgates).';

-- Índices
CREATE INDEX IF NOT EXISTS ix_extrato_parceiro          ON extrato_pontos (parceiro_id);
CREATE INDEX IF NOT EXISTS ix_extrato_tipo_transacao    ON extrato_pontos (tipo_transacao);
CREATE INDEX IF NOT EXISTS ix_extrato_criado_em         ON extrato_pontos (criado_em DESC);
CREATE INDEX IF NOT EXISTS ix_extrato_compra            ON extrato_pontos (compra_id) WHERE compra_id IS NOT NULL;

-- =============================================================================
-- 7. TABELA: resgates
--    Solicitações de resgate de bônus dos parceiros (RF06).
-- =============================================================================

CREATE TABLE IF NOT EXISTS resgates (
    id                  UUID                PRIMARY KEY DEFAULT uuid_generate_v4(),
    parceiro_id         UUID                NOT NULL,
    pontos_resgatados   INTEGER             NOT NULL,
    valor_monetario     NUMERIC(12,2)       NOT NULL,
    status              VARCHAR(50)         NOT NULL DEFAULT 'Pendente',
    aprovado_por        UUID                NULL,
    observacoes         VARCHAR(500)        NULL,
    criado_em           TIMESTAMPTZ         NOT NULL DEFAULT NOW(),
    atualizado_em       TIMESTAMPTZ         NULL,

    -- FK
    CONSTRAINT fk_resgates_parceiro
        FOREIGN KEY (parceiro_id) REFERENCES parceiros (id) ON DELETE RESTRICT,

    CONSTRAINT fk_resgates_aprovado_por
        FOREIGN KEY (aprovado_por) REFERENCES usuarios (id) ON DELETE RESTRICT,

    -- Check
    CONSTRAINT ck_resgates_pontos  CHECK (pontos_resgatados > 0),
    CONSTRAINT ck_resgates_valor   CHECK (valor_monetario > 0)
);

COMMENT ON TABLE  resgates                  IS 'Solicitações de resgate de bônus pelos parceiros.';
COMMENT ON COLUMN resgates.status           IS 'Status do resgate: Pendente, Aprovado, Rejeitado, Cancelado.';
COMMENT ON COLUMN resgates.aprovado_por     IS 'Administrador/Funcionário que aprovou o resgate.';

-- Índices
CREATE INDEX IF NOT EXISTS ix_resgates_parceiro    ON resgates (parceiro_id);
CREATE INDEX IF NOT EXISTS ix_resgates_status      ON resgates (status);
CREATE INDEX IF NOT EXISTS ix_resgates_criado_em   ON resgates (criado_em DESC);

-- =============================================================================
-- 8. TABELA: regras_pontuacao
--    Parametrização de regras de pontuação (RF06b / RN02).
-- =============================================================================

CREATE TABLE IF NOT EXISTS regras_pontuacao (
    id                                  UUID            PRIMARY KEY DEFAULT uuid_generate_v4(),
    nome                                VARCHAR(100)    NOT NULL,
    valor_compra_minimo                 NUMERIC(12,2)   NOT NULL,
    pontos_por_valor                    INTEGER         NOT NULL,
    valor_monetario_por_pontos          NUMERIC(12,2)   NOT NULL,
    pontos_para_conversao_monetaria     INTEGER         NOT NULL,
    ativo                               BOOLEAN         NOT NULL DEFAULT TRUE,
    criado_por                          UUID            NOT NULL,
    criado_em                           TIMESTAMPTZ     NOT NULL DEFAULT NOW(),
    atualizado_em                       TIMESTAMPTZ     NULL,

    -- FK
    CONSTRAINT fk_regras_pontuacao_criado_por
        FOREIGN KEY (criado_por) REFERENCES usuarios (id) ON DELETE RESTRICT,

    -- Check
    CONSTRAINT ck_regras_pontuacao_valor_minimo  CHECK (valor_compra_minimo > 0),
    CONSTRAINT ck_regras_pontuacao_pontos        CHECK (pontos_por_valor > 0),
    CONSTRAINT ck_regras_pontuacao_valor         CHECK (valor_monetario_por_pontos > 0),
    CONSTRAINT ck_regras_pontuacao_conversao     CHECK (pontos_para_conversao_monetaria > 0)
);

COMMENT ON TABLE  regras_pontuacao                                  IS 'Regras parametrizáveis de geração de pontos (configuradas pelo Administrador).';
COMMENT ON COLUMN regras_pontuacao.valor_compra_minimo              IS 'Valor em R$ de compras necessário para gerar pontos (ex: R$ 1.000,00).';
COMMENT ON COLUMN regras_pontuacao.pontos_por_valor                 IS 'Quantidade de pontos gerados ao atingir o valor mínimo (ex: 100 pontos).';
COMMENT ON COLUMN regras_pontuacao.valor_monetario_por_pontos       IS 'Valor em R$ que o parceiro recebe ao resgatar pontos (ex: R$ 50,00).';
COMMENT ON COLUMN regras_pontuacao.pontos_para_conversao_monetaria  IS 'Quantidade de pontos necessários para conversão monetária (ex: 100 pontos).';
COMMENT ON COLUMN regras_pontuacao.ativo                            IS 'Apenas uma regra ativa por vez — controlado por lógica de aplicação.';

-- Índice
CREATE INDEX IF NOT EXISTS ix_regras_pontuacao_ativo ON regras_pontuacao (ativo) WHERE ativo = TRUE;

-- =============================================================================
-- 9. TABELA: tokens_atualizacao
--    Tokens de refresh para autenticação JWT (RNF02).
-- =============================================================================

CREATE TABLE IF NOT EXISTS tokens_atualizacao (
    id              UUID            PRIMARY KEY DEFAULT uuid_generate_v4(),
    usuario_id      UUID            NOT NULL,
    token           TEXT            NOT NULL,
    expira_em       TIMESTAMPTZ     NOT NULL,
    revogado_em     TIMESTAMPTZ     NULL,
    criado_em       TIMESTAMPTZ     NOT NULL DEFAULT NOW(),

    -- FK
    CONSTRAINT fk_tokens_atualizacao_usuario
        FOREIGN KEY (usuario_id) REFERENCES usuarios (id) ON DELETE CASCADE,

    -- Unique
    CONSTRAINT uq_tokens_atualizacao_token UNIQUE (token)
);

COMMENT ON TABLE  tokens_atualizacao             IS 'Tokens de refresh para renovação da autenticação JWT.';
COMMENT ON COLUMN tokens_atualizacao.revogado_em IS 'Data/hora da revogação — NULL enquanto ativo.';

-- Índices
CREATE INDEX IF NOT EXISTS ix_tokens_atualizacao_usuario    ON tokens_atualizacao (usuario_id);
CREATE INDEX IF NOT EXISTS ix_tokens_atualizacao_expira_em  ON tokens_atualizacao (expira_em);

-- =============================================================================
-- 10. TRIGGERS: Atualização automática de atualizado_em
-- =============================================================================

CREATE OR REPLACE FUNCTION fn_set_atualizado_em()
RETURNS TRIGGER AS $$
BEGIN
    NEW.atualizado_em = NOW();
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Aplica o trigger em todas as tabelas que possuem atualizado_em
DO $$
DECLARE
    tbl TEXT;
BEGIN
    FOR tbl IN
        SELECT unnest(ARRAY[
            'usuarios',
            'funcionarios',
            'parceiros',
            'clientes',
            'compras',
            'resgates',
            'regras_pontuacao'
        ])
    LOOP
        EXECUTE format(
            'DROP TRIGGER IF EXISTS trg_%s_atualizado_em ON %I;
             CREATE TRIGGER trg_%s_atualizado_em
                BEFORE UPDATE ON %I
                FOR EACH ROW
                EXECUTE FUNCTION fn_set_atualizado_em();',
            tbl, tbl, tbl, tbl
        );
    END LOOP;
END
$$;

-- =============================================================================
-- FIM DO SCRIPT DE ESTRUTURA
-- =============================================================================
