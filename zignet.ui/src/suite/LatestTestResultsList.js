import React, { Component } from 'react';
import TestResultRow from './TestResultRow'

class LatestTestResultsList extends Component {

  render() {
    return (
      <div className="container">
        <table className="table">
          <thead>
            <tr>
              <th scope="col">Name</th>
              <th scope="col">Status</th>
              <th scope="col">History</th>
            </tr>
          </thead>
          <tbody>
            {this.props.testResults.map((testResult) =>
              <TestResultRow key={testResult.TestResultID}
                                testResult={testResult} />
            )}        
          </tbody>
        </table>
      </div>
    );
  }
}

export default LatestTestResultsList;
