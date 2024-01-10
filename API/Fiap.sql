DO $$
BEGIN 
    IF NOT EXISTS (SELECT FROM pg_tables WHERE schemaname = 'public' AND tablename = 'pagamento_status') THEN 
        CREATE TABLE public.pagamento_status (
            id int4 NOT NULL,
            nome varchar NOT NULL,
            CONSTRAINT pagamento_status_pk PRIMARY KEY (id)
        );

        ALTER TABLE public.pagamento_status OWNER TO postgres;
        GRANT ALL ON TABLE public.pagamento_status TO postgres;
    END IF; 
END $$;


DO $$
BEGIN 
    IF NOT EXISTS (SELECT FROM pg_tables WHERE schemaname = 'public' AND tablename = 'tipo_pagamento') THEN 
        CREATE TABLE public.tipo_pagamento (
            id int2 NOT NULL,
            nome varchar NULL,
            CONSTRAINT tipo_pagamento_pk PRIMARY KEY (id)
        );

        ALTER TABLE public.tipo_pagamento OWNER TO postgres;
        GRANT ALL ON TABLE public.tipo_pagamento TO postgres;
    END IF; 
END $$;


DO $$
BEGIN 
    IF NOT EXISTS (SELECT FROM pg_tables WHERE schemaname = 'public' AND tablename = 'pedido_pagamento') THEN 
        CREATE TABLE public.pedido_pagamento (
            id int4 NOT NULL GENERATED ALWAYS AS IDENTITY( INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 START 1 CACHE 1 NO CYCLE),
            pedido_id int4 NOT NULL,
            tipo_pagamento_id int2 NOT NULL,
            valor numeric NOT NULL,
            codigo_transacao varchar NULL,
            pagamento_status_id int2 NULL,
            CONSTRAINT pedido_pagamento_pk PRIMARY KEY (id),
            CONSTRAINT pedido_pagamento_un UNIQUE (pedido_id),            
            CONSTRAINT tipo_pagamento_fk FOREIGN KEY (tipo_pagamento_id) REFERENCES public.tipo_pagamento(id)
        );

        ALTER TABLE public.pedido_pagamento OWNER TO postgres;
        GRANT ALL ON TABLE public.pedido_pagamento TO postgres;
    END IF; 
END $$;





INSERT INTO public.pagamento_status (id, nome) VALUES
(1, 'Aprovado'),
(2, 'Recusado')
ON CONFLICT (id) 
DO NOTHING;


INSERT INTO public.tipo_pagamento (id, nome) VALUES
(1, 'Débito'),
(2, 'Crédito')
ON CONFLICT (id) 
DO NOTHING;





