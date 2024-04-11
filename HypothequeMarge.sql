CREATE PROCEDURE HypthequeMarge
    @hypothequeId INT,
    @montant DECIMAL
AS
BEGIN
    DECLARE @hypothequeSolde DECIMAL
    DECLARE @margeDeCreditId INT
    DECLARE @margeDeCreditSolde DECIMAL

    -- R�cup�rer le solde de l'hypoth�que.
    SELECT @hypothequeSolde = Solde
    FROM Comptes
    WHERE IdCompte = @hypothequeId

    -- V�rification de la balance de l'hypoth�que.
    IF @hypothequeSolde >= @montant
    BEGIN
        -- Retrait du compte hypoth�que.
        UPDATE Comptes SET Solde = Solde - @montant WHERE IdCompte = @hypothequeId
        -- Retrait avec succ�s
		RETURN 0; 
    END
    ELSE
    BEGIN
		-- Calcul du montant restant du retrait � partir de la marge de cr�dit.
        DECLARE @retraitMarge DECIMAL
        SET @retraitMarge = @montant - @hypothequeSolde
		
		-- Associer la marge au bon client.
        SELECT @margeDeCreditId = IdCompte, @margeDeCreditSolde = Solde
        FROM Comptes
        WHERE TypeCompte = 'Marge de cr�dit' AND IdUtilisateur = (SELECT IdUtilisateur FROM Comptes WHERE IdCompte = @hypothequeId)

        -- V�rifier si le client � une marge.
        IF @margeDeCreditId IS NULL
        BEGIN
            -- Si le client n'a pas de marge et le montant du retrait exc�de le solde du compte.
            RETURN 2; 
        END
        ELSE
        BEGIN
			-- V�rifier si la limite de cr�dit est suffisante pour effectuer le retrait.
            IF @margeDeCreditSolde >= @retraitMarge
            BEGIN
                UPDATE Comptes SET Solde = Solde - @hypothequeSolde WHERE IdCompte = @hypothequeId
                UPDATE Comptes SET Solde = Solde - @retraitMarge WHERE IdCompte = @margeDeCreditId
                RETURN 0; -- Successful withdrawal
            END
            ELSE
            BEGIN
				-- Balance insuffisante.
                RETURN 1;
            END
        END
    END
END
