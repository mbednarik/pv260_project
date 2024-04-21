import { useSuspenseQuery } from "@tanstack/react-query";
import fetchHoldings from "~/api/holdings";

const useHoldings = () =>
	useSuspenseQuery({
		queryKey: ["allHoldings"],
		queryFn: () => fetchHoldings(),
	});

export default useHoldings;
