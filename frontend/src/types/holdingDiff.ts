export type HoldingDiffsQueryParams = {
	fundId: number;
	oldHoldingDate: string;
	newHoldingDate: string;
};

export type HoldingDiff = {
	id: number;
	oldShares: number;
	sharesChange: number;
	oldWeight: number;
	weightChange: number;
	fundName: string;
	companyName: string;
	ticker?: string;
};
