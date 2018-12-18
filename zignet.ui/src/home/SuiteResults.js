import React, { Component } from 'react';
import SuiteResultChart from './SuiteResultChart'
import ListButton from './ListButton'

class SuiteResults extends Component {
  render() {
    return (
  	  <div className="container">
        <div className="text-right">
          <ListButton />
        </div>
  	  	<div className="row">
          {this.props.suiteResults.map((suiteResult) =>
            <SuiteResultChart key={suiteResult.SuiteIds[0]}
                              suiteResult={suiteResult} />
          )}
        </div>
      </div>
    );
  }
}

export default SuiteResults;
