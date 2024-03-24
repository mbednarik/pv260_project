import { Flex, Table } from "@mantine/core";
import {
	IconTriangleFilled,
	IconTriangleInvertedFilled,
} from "@tabler/icons-react";
import { HoldingDiff } from "~/types/holdingDiff";

const ICON_SIZE = 12;

type HoldingDiffTableRowProps = {
	holdingDiff: HoldingDiff;
};

const getChangeIcon = (change: number) => {
	if (change === 0) {
		return null;
	}

	return change > 0 ? (
		<IconTriangleFilled color="green" size={ICON_SIZE} />
	) : (
		<IconTriangleInvertedFilled color="red" size={ICON_SIZE} />
	);
};

const getSharesChangePercentage = (oldShares: number, sharesChange: number) => {
	if (oldShares === 0 || sharesChange === 0) {
		return 100;
	}

	return (sharesChange / oldShares) * 100;
};

export const HoldingDiffTableRow = ({
	holdingDiff: {
		fundName,
		companyName,
		ticker,
		oldShares,
		sharesChange,
		weightChange,
	},
}: HoldingDiffTableRowProps) => {
	const sharesChangePercentage = getSharesChangePercentage(
		oldShares,
		sharesChange
	);

	return (
		<Table.Tr>
			<Table.Td>{fundName}</Table.Td>
			<Table.Td>{companyName}</Table.Td>
			<Table.Td>{ticker}</Table.Td>
			<Table.Td>{oldShares + sharesChange}</Table.Td>
			<Table.Td>
				<Flex gap="sm" align="center">
					{sharesChangePercentage}%{" "}
					{getChangeIcon(sharesChangePercentage)}
				</Flex>
			</Table.Td>
			<Table.Td>
				<Flex gap="sm" align="center">
					{weightChange} {getChangeIcon(weightChange)}
				</Flex>
			</Table.Td>
		</Table.Tr>
	);
};
