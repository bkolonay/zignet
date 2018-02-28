import React, { Component } from 'react';

import SuiteResultChart from './SuiteResultChart'

class SuiteResults extends Component {

  render() {
  	const suiteResults = this.props.suiteResults;
    return (
	  <div>
        {suiteResults.map((suiteResult) =>
          <SuiteResultChart key={suiteResult.suiteResultId}
                            suiteResult={suiteResult} />
        )}
      </div>
    );
  }
}

export default SuiteResults;
