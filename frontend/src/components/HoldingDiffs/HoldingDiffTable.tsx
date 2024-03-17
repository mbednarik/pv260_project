import { Table } from "@mantine/core";
import { HoldingDiff } from "~/types/holdingDiff";
import { HoldingDiffTableRow } from "~/components/HoldingDiffs/HoldingDiffTableRow";

type HoldingDiffTableProps = {
	holdingDiffs: HoldingDiff[];
};

export const HoldingDiffTable = ({ holdingDiffs }: HoldingDiffTableProps) => (
	<Table>
		<Table.Thead>
			<Table.Tr>
				<Table.Th>Fund name</Table.Th>
				<Table.Th>Company name</Table.Th>
				<Table.Th>Ticker</Table.Th>
				<Table.Th># shares</Table.Th>
				<Table.Th>Shares change %</Table.Th>
				<Table.Th>Weight change</Table.Th>
			</Table.Tr>
		</Table.Thead>
		<Table.Tbody>
			{holdingDiffs.map(holdingDiff => (
				<HoldingDiffTableRow
					key={holdingDiff.id}
					holdingDiff={holdingDiff}
				/>
			))}
		</Table.Tbody>
	</Table>
);
